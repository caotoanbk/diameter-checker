import hmac, hashlib

# ?? Must match Communication.LicenseSecretKey in Communication.cs ?????????????
LICENSE_SECRET = b"D!@m3t3r#Ch3ck3r$2024"

KNOWN_CPU_IDS = [
    "BFEBFBFF000306C3",
    "BFEBFBFF000906E9",
    "BFEBFBFF000A0653",
    "BFEBFBFF000B06F2",  # current dev machine
]

def compute_hmac(cpu_id: str) -> str:
    return hmac.new(LICENSE_SECRET, cpu_id.encode("utf-8"), hashlib.sha256).hexdigest()

entries = []
for cpu_id in KNOWN_CPU_IDS:
    signed = cpu_id + ":" + compute_hmac(cpu_id)
    entries.append(signed)
    print("License:", signed)

out_path = r"D:\Projects\diameter-checker\Diameter_Checker\config.ini"
with open(out_path, "w") as f:
    f.write("\n".join(entries) + "\n")
print("\nconfig.ini written to", out_path)
