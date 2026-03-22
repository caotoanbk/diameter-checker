# Diameter Checker — Technical Documentation

## Table of Contents
1. [Overview](#1-overview)
2. [System Architecture](#2-system-architecture)
3. [Project Structure](#3-project-structure)
4. [Application Startup Flow](#4-application-startup-flow)
5. [Single Product Test Sequence](#5-single-product-test-sequence)
6. [Serial Communication Protocol](#6-serial-communication-protocol)
7. [Timer Reference](#7-timer-reference)
8. [Database Schema & Queries](#8-database-schema--queries)
9. [License System](#9-license-system)
10. [Configuration Files](#10-configuration-files)
11. [Key Components](#11-key-components)
12. [Maintenance Guide](#12-maintenance-guide)

---

## 1. Overview

**Diameter Checker** is a Windows desktop application (.NET Framework 4.5.2 / WinForms) used on a manufacturing line to:

- Measure the **diameter (roundness)** of mechanical parts using two air-pressure sensors (A1, A2)
- Measure the **weight** of each part via a serial weighing scale
- **Judge each part PASS or FAIL** based on configurable tolerance limits
- **Track statistics** across a production run (total processed, PASS count, FAIL count, PP/PPK indices)
- **Store all results** in a SQL Server Express database
- **Export data** to Excel for reporting

---

## 2. System Architecture

```
???????????????????????????????????????????????????????????????????
?                        Windows PC                               ?
?                                                                 ?
?  ????????????????    SerialPort 1     ????????????????????????  ?
?  ?  frmMain.cs  ??????????????????????? Air Pressure Sensor  ?  ?
?  ?  (Main UI)   ?                     ? (A1 + A2 combined)   ?  ?
?  ?              ?    SerialPort 2     ????????????????????????  ?
?  ?              ??????????????????????????????????????????????  ?
?  ?              ?                     ?   Weighing Scale     ?  ?
?  ?              ?                     ????????????????????????  ?
?  ?              ?                                               ?
?  ?              ??????????????????????????????????????????????  ?
?  ?              ?   SQL Server        ?  DB1 (SQL Express)   ?  ?
?  ????????????????                     ????????????????????????  ?
?                                                                 ?
?  Communication.cs — global shared state (static fields)         ?
???????????????????????????????????????????????????????????????????
```

### Technology Stack

| Layer | Technology |
|---|---|
| UI | WinForms (.NET Framework 4.5.2) |
| Language | C# |
| Database | SQL Server Express (`.\SQLEXPRESS`, database `DB1`) |
| Serial | `System.IO.Ports.SerialPort` (two instances) |
| Charts | `System.Windows.Forms.DataVisualization.Charting` |
| Excel export | `ClosedXML` + `Microsoft.Office.Interop.Excel` |
| License | HMAC-SHA256 signed CPU ID in `config.ini` |
| DB connection | `ConfigurationManager` ? `App.config` |

---

## 3. Project Structure

```
diameter-checker/
?
??? Diameter_Checker/
?   ??? frmMain.cs              ? Main form: UI, measurement logic, timers
?   ??? Communication.cs        ? Global state, serial helpers, DB helpers,
?   ?                              weight frame parser, license verifier
?   ??? ComSetting.cs           ? Serial port configuration dialog
?   ??? SettingLogin.cs         ? Admin/Developer login gate for settings
?   ??? PasswordToStart.cs      ? Password prompt shown before each test start
?   ??? ChangePassWord.cs       ? Password management form
?   ??? ExportData.cs           ? Excel export dialog
?   ??? App.config              ? DB connection string (edit without rebuild)
?   ??? config.ini              ? HMAC-signed license entries (one per line)
?   ??? Diameter_Checker.csproj
?
??? gen_license.py              ? Generate license for current machine
??? gen_license_all.py          ? Generate licenses for all known CPU IDs
??? DOCUMENTATION.md            ? This file
```

---

## 4. Application Startup Flow

```
Program.Main()
    ?
    ??? Application.Run(new frmMain())
              ?
              ??? frmMain constructor
                      ?
                      ??? InitializeComponent()   ? build UI controls
                      ??? Register DataReceive()  ? serialport1.DataReceived
                      ??? Register DataReceive2() ? serialport2.DataReceived

              ??? Form1_Load()
                      ?
                      ??? 1. Load COM port settings from DB (ComportSetting table)
                      ??? 2. Try to connect serialport1 (A1/A2 sensor)
                      ??? 3. Try to connect serialport2 (weighing scale)
                      ??? 4. Load product list from DB (ProductSetting table)
                      ??? 5. Load and display last 21 records (Data table)
                      ??? 6. Initialize all charts (A1, A2, Weight, D1, D2)
                      ??? 7. LICENSE CHECK
                      ?       ??? Read CPU ProcessorID via WMI
                      ?       ??? Verify against config.ini entries (HMAC-SHA256)
                      ?       ??? If invalid ? MessageBox + Application.Exit()
                      ??? 8. calculatePPandPPKvalue() ? compute initial PP/PPK
```

---

## 5. Single Product Test Sequence

### Prerequisites
- Serial ports connected and open
- Product model selected in `cmbModel`
- Operator clicks **Start** ? enters password in `PasswordToStart`

### Full sequence diagram

```
OPERATOR places product under sensors
         ?
         ?
???????????????????????????????????????????????????????????????????????
?  PHASE 1 — DIAMETER MEASUREMENT (SerialPort 1)                      ?
?                                                                     ?
?  serialport1 fires DataReceive() continuously (event-driven)        ?
?      ?                                                              ?
?      ??? InputData += ReadExisting()  [strips \r\n]                 ?
?               ?                                                     ?
?               ??? SetText(InputData)                                ?
?                       ?                                             ?
?                       ??? Validate frame: 27 chars, starts "A1",   ?
?                           has "A2" at byte 14, OK/NG at 11 & 25    ?
?                               ?                                     ?
?                               ??? tmrDisplayData.Enabled = true    ?
?                                         ?                           ?
?                                  [50ms later]                       ?
?                                  tmrDisplayData_Tick()              ?
?                                         ?                           ?
?  ?? A1 processing ???????????????????????                           ?
?  A1MeasuredValue = serialData[3..8]     ?                           ?
?  A1Result        = serialData[11..12]   ?                           ?
?                                         ?                           ?
?  if A1Value < DetectionLevel - offset:  ? ? product detected       ?
?      tmrEnableReadA1Data.Start()        ?                           ?
?      [X seconds — configured in UI]     ?                           ?
?      tmrEnableReadA1Data_Tick()         ?                           ?
?          A1RecevingData = true          ? ? allow recording         ?
?                                         ?                           ?
?  While A1RecevingData == true:          ?                           ?
?      Update A1Maximum/MinimumValue      ? ? track max/min           ?
?      saveA1BufferData() ? DB            ?                           ?
?      chart1Display()                    ?                           ?
?                                         ?                           ?
?  When A1Value >= DetectionLevel:        ? ? product leaves sensor   ?
?      getA1BufferData() ? read from DB  ?                           ?
?      deleteA1BufferData()               ?                           ?
?      A1EnableSave = true               ? ? A1 done ?               ?
?                                         ?                           ?
?  ?? A2 processing (parallel, same frame)?                           ?
?  A2MeasuredValue = serialData[17..22]   ?                           ?
?  A2Result        = serialData[25..26]   ?                           ?
?  (same detection logic as A1)           ?                           ?
?      A2EnableSave = true               ? ? A2 done ?               ?
???????????????????????????????????????????????????????????????????????
         ?
         ?
???????????????????????????????????????????????????????????????????????
?  PHASE 2 — WEIGHT MEASUREMENT (SerialPort 2)                        ?
?                                                                     ?
?  serialport2 fires DataReceive2() (event-driven, background thread) ?
?      ?                                                              ?
?      ??? lock(_inputData2Lock)                                      ?
?          InputData2 += ReadExisting()  [strips \r\n]                ?
?               ?                                                     ?
?               ??? BeginInvoke ? SetText2() on UI thread             ?
?                       ?                                             ?
?                       ??? ParseWeightFrame(serialData2)             ?
?                               ?                                     ?
?                               ??? Header: " ST,GS,+"               ?
?                               ??? Extract bytes[8..18] ? value      ?
?                               ??? Strip "g", parse float / 10       ?
?                               ??? Compare vs minWeight / maxWeight  ?
?                                       ?                             ?
?                                       ??? OK  ? txtWeightResult = green ?
?                                       ??? NG  ? txtWeightResult = red   ?
?                                                                     ?
?  receivedWeightFlg = true ?                                         ?
???????????????????????????????????????????????????????????????????????
         ?
         ?
???????????????????????????????????????????????????????????????????????
?  PHASE 3 — DECISION (tmrDisplayData_Tick checks every 50ms)         ?
?                                                                     ?
?  Condition: receivedWeightFlg == true                               ?
?          && A1EnableSave == true                                    ?
?          && A2EnableSave == true                                    ?
?                  ?                                                  ?
?                  ??? CheckAndMakeDecision()                         ?
?                          ?                                          ?
?  if A1Result=="OK" && A2Result=="OK" && WeightResult=="OK"          ?
?      ? Judge = "PASS"                                               ?
?      ? totalPASS++, cntProductInSet++                               ?
?      ? If cntProductInSet >= target ? stopWorking()                 ?
?  else                                                               ?
?      ? Judge = "FAIL"                                               ?
?      ? totalFAIL++                                                  ?
?      ? stopWorking()                                                ?
?      ? PasswordToStart popup (must re-enter to continue)            ?
?                                                                     ?
?  ? tmrDisplayJudge.Enabled = true  [500ms delay before showing]    ?
?  ? totalProcessed++                                                 ?
?  ? saveData() ? INSERT into Data table                              ?
?  ? loadData() ? refresh DataGridView                                ?
?  ? calculatePPandPPKvalue() ? update PP/PPK charts                  ?
?  ? ClearAllData() ? reset UI for next product                       ?
???????????????????????????????????????????????????????????????????????
         ?
         ?
   Ready for next product
```

### Key state flags during a test cycle

| Flag | Set to `true` when | Set to `false` when |
|---|---|---|
| `Communication.start` | Operator clicks Start | Operator clicks Stop / FAIL |
| `A1RecevingData` | `tmrEnableReadA1Data` fires | Product leaves A1 sensor |
| `A1Detected` | A1 value drops below detection level | Product leaves A1 sensor |
| `A1EnableSave` | Product fully measured by A1 | After `saveData()` |
| `A2RecevingData` | `tmrEnableReadA2Data` fires | Product leaves A2 sensor |
| `A2EnableSave` | Product fully measured by A2 | After `saveData()` |
| `receivedWeightFlg` | Valid weight frame received | After `saveData()` |
| `enableClearData` | `CheckAndMakeDecision()` completes | `ClearAllData()` runs |

---

## 6. Serial Communication Protocol

### SerialPort 1 — Air Pressure Sensor (A1 + A2)

| Parameter | Value |
|---|---|
| Data source | `ComportSetting` table in DB |
| Frame length | **27 characters** |
| Frame format | `A1xxxxxxOKA2xxxxxxOK` |

```
Position:  0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26
Content:   A  1  _  v  v  v  v  v  v  _  _  R  R  _  A  2  _  v  v  v  v  v  v  _  _  R  R

A1 prefix:     [0..1]  = "A1"
A1 value:      [3..8]  = measured value (6 chars)
A1 result:     [11..12] = "OK" or "NG"
A2 prefix:     [14..15] = "A2"
A2 value:      [17..22] = measured value (6 chars)
A2 result:     [25..26] = "OK" or "NG"
```

### SerialPort 2 — Weighing Scale

| Parameter | Value |
|---|---|
| Data source | `ComportSetting` table in DB |
| Frame length | **19 characters** |
| Frame header | `" ST,GS,+"` (8 chars) |

```
Position:  0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18
Content:   _  S  T  ,  G  S  ,  +  v  v  v  v  v  v  v  v  v  v  v

Header:    [0..7]  = " ST,GS,+"
Value:     [8..18] = weight string (11 chars), e.g. "   0123.4g "
```

**Parsing logic** (in `Communication.ParseWeightFrame()`):
1. Trim whitespace
2. Strip `"g"` unit suffix
3. Remove decimal point, divide by 10 ? float value in grams

> ?? **Changing the scale machine**: update only the constants at the top of `Communication.cs` (`WeightFrameHeader`, `WeightFrameLength`, `WeightValueOffset`, etc.) and the body of `ParseWeightFrame()`. No other files need changes.

---

## 7. Timer Reference

| Timer | Interval | Pattern | Purpose |
|---|---|---|---|
| `tmrDisplayData` | 50 ms | One-shot | Processes one validated A1/A2 frame. Disabled at entry, re-enabled by `SetText()` when a valid frame arrives. |
| `tmrDisplayJudge` | 500 ms | One-shot | Delays PASS/FAIL display by 500ms after decision so operator can see the result before any popup appears. |
| `tmrEnableReadA1Data` | 0.5–3 s (UI) | One-shot | Waits after A1 product detection for sensor signal to stabilize. Sets `A1RecevingData = true` on fire. |
| `tmrEnableReadA2Data` | 0.5–3 s (UI) | One-shot | Same as above for A2 sensor. |
| `tmrConnectionStatus` | 1000 ms | Periodic | Checks both serial ports every second. Auto-reconnects if `AutoReconnect` is enabled. Also syncs `tmrEnableReadA1/A2Data` interval from the UI combo box. |
| `tmrDateTime` | 1000 ms | Periodic | Updates `Communication.Date/Time` and the on-screen clock label. |
| `tmrA1DetectRemoveObject` | 1000 ms | One-shot | **Not implemented** — reserved for future auto-reset when A1 object is removed. |
| `tmrA2DetectRemoveObject` | 1000 ms | One-shot | **Not implemented** — reserved for future auto-reset when A2 object is removed. |
| `tmrRefreshChart` | 10 ms | Periodic | **Not implemented** — reserved for future live chart refresh. |
| `tmrRefreshDataGridView` | 10 ms | One-shot | Refreshes DataGridView and recalculates PP/PPK after `ExportData` dialog closes. |

---

## 8. Database Schema & Queries

### Tables used

#### `Data` — measurement results
| Column | Type | Description |
|---|---|---|
| `ID` | varchar | Format: `"HL" + totalProcessed` |
| `model` | varchar | Product model name |
| `weight` | varchar | Weight value as string |
| `A1MaxValue` | varchar | Max measured value from A1 |
| `A1MinValue` | varchar | Min measured value from A1 |
| `A1Roundness` | varchar | A1Max - A1Min (computed) |
| `A1Result` | varchar | "OK" or "NG" |
| `A2MaxValue` | varchar | Max measured value from A2 |
| `A2MinValue` | varchar | Min measured value from A2 |
| `A2Roundness` | varchar | A2Max - A2Min (computed) |
| `A2Result` | varchar | "OK" or "NG" |
| `Date` | varchar | Format: `yyyy-MM-dd` |
| `Time` | varchar | Format: `H:mm` |
| `Judge` | varchar | "PASS" or "FAIL" |
| `TotalProcessed` | int | Running total at time of save |
| `TotalPASS` | int | Running PASS count at time of save |
| `TotalFAIL` | int | Running FAIL count at time of save |
| `Display` | bit | 1 = visible in main grid |

#### `ProductSetting` — product tolerance configuration
| Column | Description |
|---|---|
| `model` | Product model name (PK) |
| `A1DetectionValue` | Threshold below which A1 detects an object |
| `A2DetectionValue` | Threshold below which A2 detects an object |
| `A1MinimumOffset` | Lower tolerance for A1 diameter |
| `A1MaximumOffset` | Upper tolerance for A1 diameter |
| `A2MinimumOffset` | Lower tolerance for A2 diameter |
| `A2MaximumOffset` | Upper tolerance for A2 diameter |
| `minWeight` | Minimum acceptable weight |
| `maxWeight` | Maximum acceptable weight |

#### `ComportSetting` — serial port configuration
| Column | Description |
|---|---|
| `comPort` | COM port name for sensor (e.g. `COM3`) |
| `baudrate` | Baud rate for sensor |
| `comPort2` | COM port name for scale |
| `baudrate2` | Baud rate for scale |

#### `A1BufferData` / `A2BufferData` — temporary measurement buffer
Used to accumulate max/min readings while a product is under the sensor. Cleared after each measurement cycle.

---

## 9. License System

The software validates the machine's CPU at startup using HMAC-SHA256.

### Validation flow
```
Form1_Load()
    ?
    ??? GetMachineFingerprint()
    ?       ??? WMI: win32_processor.processorID ? e.g. "BFEBFBFF000B06F2"
    ?
    ??? Read each line of config.ini
    ?       format: <cpuId>:<hmac-sha256-hex>
    ?
    ??? VerifyLicense(line, machineCpuId)
            ??? Split on ':'
            ??? Check cpuId matches this machine
            ??? Recompute HMAC-SHA256(cpuId, LicenseSecretKey)
            ??? Compare with stored HMAC
                    ??? Match ? licensed ?
                    ??? No match ? MessageBox + Application.Exit()
```

### Adding a new licensed machine

**Option A — Run on the target machine:**
```bash
python gen_license.py
# Outputs a signed line; add it to config.ini
```

**Option B — Generate offline if CPU ID is known:**
1. Add the CPU ID to `KNOWN_CPU_IDS` in `gen_license_all.py`
2. Run `python gen_license_all.py` ? rewrites `config.ini` with all entries
3. Deploy updated `config.ini` alongside the `.exe`

---

## 10. Configuration Files

### `App.config` (deployed as `Diameter_Checker.exe.config`)
Controls the database connection. **Edit this file without rebuilding.**
```xml
<connectionStrings>
  <add name="DB1"
       connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=DB1;..."
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### `config.ini`
Controls which machines are licensed. **Deployed alongside the `.exe`.**
```
BFEBFBFF000306C3:a6ee9a64356449d42581f94a10b35e0c12e8e729e02cc7ac7a0977e01564e8f6
BFEBFBFF000906E9:7be838dfc9b42303c4ed1970edcd1f731555063cc8c65a6013df30649fdf26b4
BFEBFBFF000A0653:09ae20a3ae72456b2ea2d3d0a124a3daaec3e1a398afda9c3ecb53d68d826766
BFEBFBFF000B06F2:ef729cc14a1cc7112d1d1e8ad45b30956a1dbe9c6c29469da12273c3d299f814
```
One line per licensed machine. Each line is `<CPU_ID>:<HMAC-SHA256>`.

---

## 11. Key Components

### `Communication.cs` — Global state & helpers
- All measurement state is stored as `public static` fields
- `CreateConnection()` — opens a new `SqlConnection` (callers use `using`)
- `ConnectSerial()` / `ConnectSerial2()` — configure and open serial ports
- `ParseWeightFrame(string raw)` — parses one weight scale frame; all machine-specific constants are at the top of the file
- `GetMachineFingerprint()` — reads CPU ID via WMI
- `VerifyLicense(string line, string cpuId)` — HMAC-SHA256 license check
- `load_ComSetting()` — loads COM port config from DB

### `frmMain.cs` — Main form
- `DataReceive()` — serial background thread handler for sensor data
- `DataReceive2()` — serial background thread handler for scale data (thread-safe with `lock`)
- `SetText()` — validates A1/A2 frames on UI thread, triggers `tmrDisplayData`
- `SetText2()` — parses weight frames on UI thread via `BeginInvoke`
- `tmrDisplayData_Tick()` — core measurement processing loop (50ms)
- `CheckAndMakeDecision()` — final PASS/FAIL logic and DB save
- `calculatePPandPPKvalue()` — computes PP/PPK for A1, A2, and Weight
- `saveData()` — inserts one measurement record to `Data` table
- `loadData()` — loads last 21 records into DataGridView

---

## 12. Maintenance Guide

### Changing the weighing scale machine
Edit **`Communication.cs`** only — update these constants:
```csharp
private const string WeightFrameHeader       = " ST,GS,+";  // ? header marker
private const int    WeightFrameLength       = 19;           // ? total frame length
private const int    WeightValueOffset       = 8;            // ? where the number starts
private const int    WeightValueLength       = 11;           // ? how many chars for the number
private const string WeightUnitSuffix        = "g";          // ? unit to strip
private const float  WeightValueScaleDivisor = 10f;          // ? divisor after stripping decimal
```
If the format is more complex, update `ParseWeightFrame()` body as well. `frmMain.cs` requires no changes.

### Changing the SQL Server instance
Edit `Diameter_Checker.exe.config` (not the source code):
```xml
connectionString="Data Source=NEW_SERVER\SQLEXPRESS;Initial Catalog=DB1;..."
```

### Adding a new product model
Use the COM Setting dialog ? Product Setting tab, or insert directly into `ProductSetting` table.

### Adding a licensed machine
Run `python gen_license.py` on the target machine, copy the output line into `config.ini`.

### Changing the HMAC secret key
1. Update `LicenseSecretKey` in `Communication.cs`
2. Update `LICENSE_SECRET` in both `gen_license.py` and `gen_license_all.py`
3. Regenerate all `config.ini` entries with `gen_license_all.py`
4. Rebuild and redeploy
