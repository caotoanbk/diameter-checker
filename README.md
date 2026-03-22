# Diameter Checker

A WinForms application (.NET Framework 4.5.2) for measuring the diameter (roundness) and weight of manufactured parts on a production line, with automatic PASS/FAIL judgment and PP/PPK statistical reporting.

> **Full technical documentation** (architecture, timer reference, serial protocol, DB schema) is in [`DOCUMENTATION.md`](./DOCUMENTATION.md).

---

## Requirements

### Development machine
| Requirement | Version |
|---|---|
| Visual Studio | 2019 or later |
| .NET Framework | 4.5.2 |
| Python | 3.x (for license generation scripts only) |
| NuGet packages | Restored automatically on build |

### Production machine
| Requirement | Notes |
|---|---|
| Windows | 7 or later (x86 or x64) |
| .NET Framework | 4.5.2 runtime |
| SQL Server Express | `.\SQLEXPRESS`, database named `DB1` |
| Serial port 1 | Air pressure sensor (A1 + A2) |
| Serial port 2 | Weighing scale |

---

## Build

```bash
# Clone the repository
git clone https://github.com/caotoanbk/diameter-checker.git
cd diameter-checker

# Open in Visual Studio and build (NuGet packages restore automatically)
# — or build from the command line:
msbuild Diameter_Checker.sln /p:Configuration=Release
```

Output: `Diameter_Checker\bin\Release\`

---

## Database Setup

1. Install **SQL Server Express** on the target machine.
2. Create a database named **`DB1`**.
3. Run the following SQL to create the required tables:

```sql
-- Measurement results
CREATE TABLE Data (
    ID              VARCHAR(50),
    model           VARCHAR(100),
    weight          VARCHAR(50),
    A1MaxValue      VARCHAR(50),
    A1MinValue      VARCHAR(50),
    A1Roundness     VARCHAR(50),
    A1Result        VARCHAR(10),
    A2MaxValue      VARCHAR(50),
    A2MinValue      VARCHAR(50),
    A2Roundness     VARCHAR(50),
    A2Result        VARCHAR(10),
    Date            VARCHAR(20),
    Time            VARCHAR(10),
    Judge           VARCHAR(10),
    TotalProcessed  INT,
    TotalPASS       INT,
    TotalFAIL       INT,
    Display         BIT DEFAULT 1
);

-- Product tolerance settings
CREATE TABLE ProductSetting (
    model              VARCHAR(100) PRIMARY KEY,
    A1DetectionValue   VARCHAR(50),
    A2DetectionValue   VARCHAR(50),
    A1MinimumOffset    VARCHAR(50),
    A1MaximumOffset    VARCHAR(50),
    A2MinimumOffset    VARCHAR(50),
    A2MaximumOffset    VARCHAR(50),
    minWeight          VARCHAR(50),
    maxWeight          VARCHAR(50)
);

-- Serial port configuration
CREATE TABLE ComportSetting (
    comPort   VARCHAR(20),
    baudrate  VARCHAR(10),
    comPort2  VARCHAR(20),
    baudrate2 VARCHAR(10)
);

INSERT INTO ComportSetting (comPort, baudrate, comPort2, baudrate2)
VALUES ('COM3', '9600', 'COM4', '19200');

-- Temporary measurement buffers (cleared after each product cycle)
CREATE TABLE A1BufferData (
    A1MaxValue VARCHAR(50),
    A1MinValue VARCHAR(50),
    A1Result   VARCHAR(10)
);

CREATE TABLE A2BufferData (
    A2MaxValue VARCHAR(50),
    A2MinValue VARCHAR(50),
    A2Result   VARCHAR(10)
);
```

---

## Configuration

### 1. Database connection — `Diameter_Checker.exe.config`

Edit the connection string to match the target SQL Server instance. **No rebuild required.**

```xml
<connectionStrings>
  <add name="DB1"
       connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=DB1;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

| Scenario | Change |
|---|---|
| Different SQL Server instance name | `Data Source=MY_SERVER\SQLEXPRESS` |
| SQL login instead of Windows auth | Replace `Integrated Security=True` with `User Id=sa;Password=xxx` |
| Database on a remote server | `Data Source=192.168.1.10` |

---

### 2. Serial ports — via application UI

COM port and baud rate settings are stored in the `ComportSetting` table and can be changed at runtime via the **Communication Setting** menu inside the app.

| Port | Connected to | Typical baud rate |
|---|---|---|
| COM port 1 | Air pressure sensor (A1 + A2) | 9600 |
| COM port 2 | Weighing scale | 19200 |

---

### 3. License — `config.ini`

The application validates the machine's **CPU ID** at startup using HMAC-SHA256. The `config.ini` file must be present next to the `.exe` and contain at least one valid signed entry for the current machine.

#### Licensing a new machine

**Option A — Run directly on the target machine** (requires Python):
```bash
python gen_license.py
```
This auto-detects the CPU ID and **overwrites** `config.ini` with a single signed entry.

**Option B — Generate offline** if the CPU ID is already known:
1. Add the CPU ID to `KNOWN_CPU_IDS` in `gen_license_all.py`
2. Run:
   ```bash
   python gen_license_all.py
   ```
3. Copy the updated `config.ini` to the deployment folder

#### `config.ini` format
```
# One line per licensed machine:
<CPU_ID>:<HMAC-SHA256-hex>

# Example:
BFEBFBFF000306C3:a6ee9a64356449d42581f94a10b35e0c12e8e729e02cc7ac7a0977e01564e8f6
```

---

## Deployment

Copy the following files to the installation folder on the target machine:

```
Diameter_Checker\bin\Release\
?
??? Diameter_Checker.exe
??? Diameter_Checker.exe.config     ? edit DB connection string here
??? config.ini                      ? edit licensed CPU IDs here
??? ClosedXML.dll
??? DocumentFormat.OpenXml.dll
??? ExcelNumberFormat.dll
??? (other .dll dependencies)
```

> ? Both `Diameter_Checker.exe.config` and `config.ini` can be updated on-site **without rebuilding** the application.

---

## Usage

1. Launch `Diameter_Checker.exe`
2. Go to **Menu ? Communication** to verify serial port connection status
3. Select the **product model** from the dropdown
4. Set the **number of products per set** (`numProductInSet`)
5. Click **Start** ? enter the operator password
6. Place each part under the sensors — the app measures automatically
7. Results are saved to the database after each part
8. Click **Export** to export data to Excel

### PASS / FAIL logic

A part is judged **PASS** only when all three conditions are met:

| Measurement | Condition |
|---|---|
| A1 roundness | Within `[A1MinimumOffset, A1MaximumOffset]` |
| A2 roundness | Within `[A2MinimumOffset, A2MaximumOffset]` |
| Weight | Within `[minWeight, maxWeight]` |

On any **FAIL**, the line stops and a password prompt appears before the next test can start.

---

## Changing the Weighing Scale

If the scale hardware is replaced and the serial frame format changes, edit **only `Communication.cs`**:

```csharp
// All scale protocol constants are grouped here — no other file needs changes
private const string WeightFrameHeader       = " ST,GS,+";   // frame start marker
private const int    WeightFrameLength       = 19;            // total frame length (chars)
private const int    WeightValueOffset       = 8;             // index where numeric value starts
private const int    WeightValueLength       = 11;            // length of numeric value field
private const string WeightUnitSuffix        = "g";           // unit string to strip
private const float  WeightValueScaleDivisor = 10f;           // divisor after removing decimal
```

---

## Troubleshooting

| Symptom | Likely cause | Fix |
|---|---|---|
| `"Unlicensed Machine"` on startup | `config.ini` missing or wrong CPU ID | Run `gen_license.py` on this machine |
| `"Please check the connection to your Database!"` | SQL Server not running or wrong connection string | Start SQL Server Express; check `exe.config` |
| `"Not Connected"` label in red | Serial port not found or already in use | Check COM port number in Communication Setting |
| Weight always reads 0 or blank | Scale baud rate mismatch | Match baud rate in `ComportSetting` to scale hardware |
| App opens then immediately closes | License check failed (no MessageBox seen) | Run from command prompt to see exit message |
| DataGridView shows no data | `Data` table empty or `Display` column = 0 | Check DB; verify `Display = 1` on records |

---

## Repository Structure

```
diameter-checker/
?
??? Diameter_Checker/
?   ??? frmMain.cs              Main form — UI, measurement logic, timers
?   ??? Communication.cs        Global state, serial helpers, license, weight parser
?   ??? ComSetting.cs           Serial port configuration dialog
?   ??? ExportData.cs           Excel export dialog
?   ??? PasswordToStart.cs      Operator password prompt
?   ??? ChangePassWord.cs       Password management
?   ??? App.config              DB connection string
?   ??? config.ini              HMAC-signed license entries
?   ??? Diameter_Checker.csproj
?
??? gen_license.py              Generate license for current machine
??? gen_license_all.py          Generate licenses for all known CPU IDs
??? README.md                   This file
??? DOCUMENTATION.md            Full technical documentation
??? Diameter_Checker.sln
```
