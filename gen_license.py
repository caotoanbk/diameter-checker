import subprocess, hmac, hashlib

# ?? Must match Communication.LicenseSecretKey in Communication.cs ?????????????
LICENSE_SECRET = b"D!@m3t3r#Ch3ck3r$2024"

def compute_hmac(cpu_id: str) -> str:
    return hmac.new(LICENSE_SECRET, cpu_id.encode("utf-8"), hashlib.sha256).hexdigest()

# Read CPU ID from WMI
cpu = subprocess.check_output("wmic cpu get ProcessorId /value", shell=True).decode()
cpu_id = [l.split("=")[1].strip() for l in cpu.splitlines() if "=" in l][0]

signed = cpu_id + ":" + compute_hmac(cpu_id)

print("CPU ID     :", cpu_id)
print("HMAC-SHA256:", compute_hmac(cpu_id))
print("License    :", signed)

out_path = r"D:\Projects\diameter-checker\Diameter_Checker\config.ini"
with open(out_path, "w") as f:
    f.write(signed + "\n")
print("config.ini written to", out_path)

