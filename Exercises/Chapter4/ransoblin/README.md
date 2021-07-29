# Ransoblin (Ransomware Bokoblin)

Ransoblin is a simple ransomware implementation which encrypts or decrypts the files given as a parameter. It’s a .NET Framework and/or .NET Core application, it can be compiled as both. Ransoblin simply encrypts and decrypts the file content using AES symmetrical algorithm with hard-coded keys and IV. It could accept these key and IV as parameter in the future releases. It doesn’t support multiple file encryption, boot manipulation or file delete intentionally due to safely exercise ransom scenario. It can be utilised with Petaq C2 in memory, or other C2 solutions such as Cobalt Strike or Metasploit Framework.  

# TODO
* Adding Volume Shadow Copy and File Delete Option
* Accepting KEY/IV, or Generating Them On-The-Fly
* Recursive File & Directory Encryption
