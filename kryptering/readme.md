# Kryptering

Dette repository omhandleremnet kryptering til brug i undervisningen.

[Microsoft Doc's omkring kryptering](https://docs.microsoft.com/en-us/dotnet/standard/security/cryptography-model)

.NET understøtter en rækker kryptrings algoritmer og funktionerne de kan finde under følgende namespace:

```csharp
using System.Security.Cryptography;
```

## Download

[Klik her for at hente koden](https://github.com/devcronberg/).

## Symmetrisk kryptering

Kryptering vha. symmetriske algoritmer betyder at samme nøgle (kode) bruges ved kryptering og dekryptering. Symmetrisk kryptering er ofte mere effektiv ift. cpu tid da algoritmen er mere simpel i modsætning til asymmetrisk kryptering, se næste afsnit.

.NET har implmenteret følgende symmetriske krypterings algoritmer:

```csharp
System.Security.Cryptography.Aes
System.Security.Cryptography.DES
System.Security.Cryptography.RC2
System.Security.Cryptography.Rijndael //<-- default
System.Security.Cryptography.TripleDES
```

[Microsoft doc's symmetrisk kryptering](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.symmetricalgorithm)



### Brug symmetrisk kryptering

Herunder er en kort beskrivelse af de punkter du skal igennem for at bruge symmetrisk kryptering.

1. Lav et `Stream` object til at skrive og læse fra

2. Lav en instans af `SymmetricAlgorithm`, fx:

    ```csharp
    var algorithm = RijndaelManaged.Create();
    var algorithm = TripleDES.Create();
    var algorithm = AesManaged.Create();
    var algorithm = new RC2CryptoServiceProvider();
    var algorithm = new DESCryptoServiceProvider();
    ```

3. Lav en `ICryptoTransform` object via either

    ```csharp
    SymmetricAlgorithm.CreateEncryptor()
    SymmetricAlgorithm.CreateDecryptor()
    ```

4. Lav en `CryptoStream`

5. Skriv til/læs fra `CryptoStream` for at kryptere/de-kryptere.

Herunder er et eksempel på kryptering:

```csharp
string cypherText = null;
using (MemoryStream output = new MemoryStream())
{
    using (CryptoStream crypto = new CryptoStream(output, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
    {
        var bytes = Encoding.UTF8.GetBytes("hemmelig tekst");
        crypto.Write(bytes, 0, bytes.Length);
    }

    cypherText = Convert.ToBase64String(output.ToArray());
}
```

## Asymmetrisk kryptering
I modsætning til symmetrisk kryptering så benytter asymmetrisk kryptering ikke samme nøgle til kryptering og de-kryptering. I stedet bruges en `privat` nøgle til dekryptering og en `offentlig` nøgle til kryptering. På den måde kan alle sende krypteret indhold til modtageren, men det er kun modtageren som har nøglen til at dekryptere med.
Den asymmetriske algoritme (matematik) er langt mere kompleks sammenlignet med symmetrisk kryptering og derfor er asymmetrisk kryptering også at betrægte som mere tidskrævende/cpu tung.

Du kan læse mere om asymmetrisk kryptering på Microsoft Doc's:

[Microsoft doc's asymmetrisk kryptering](
https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.asymmetricalgorithm)

.NET understøtter følgende asymmetriske krypterings algoritmer:

```csharp
System.Security.Cryptography.DSA
System.Security.Cryptography.ECDiffieHellman
System.Security.Cryptography.ECDsa
System.Security.Cryptography.RSA
```

### Brug asymmetrisk kryptering

1. Opret et RSACryptoProvider object

2. Hent/set nøglen ved at kalde

    ```csharp
    RSACryptoProvider.ExportParameters()
    RSACryptoProvider.ImportParameters()
    ```

3. Convert text til bytes eller bytes til text

4. Krypter/dekrypter ved at kalde:

    ```csharp
    RSACryptoProvider.Encrypt()
    RSACryptoProvider.Decrypt()
    ```

5. Convert text til bytes eller bytes til text

### Eksempel

```csharp
var text = "Dette er en hemmelig tekst";

RSACryptoServiceProvider decryptor = new RSACryptoServiceProvider();
RSAParameters publicKey = decryptor.ExportParameters(false);

// Encrypt
RSACryptoServiceProvider encryptor = new RSACryptoServiceProvider();
encryptor.ImportParameters(publicKey);
byte[] messageBytes = Encoding.Unicode.GetBytes(text);
byte[] encryptedMessage = encryptor.Encrypt(messageBytes, false);
var cypher = Convert.ToBase64String(encryptedMessage);

// Decrypt
byte[] decryptedBytes = decryptor.Decrypt(Convert.FromBase64String(cypher), false);
var clear = Encoding.Unicode.GetString(decryptedBytes);
```

## Hashing

Hashing algoritmer bruges til at validere at data er uændret. Algoritmen går kun én vej. Hash værdien kan vil være den samme for ens data, mens den vil være forskellig for data som ikke er ens. Hash værdien er endvidere en envejs "data reduktion". Det er derfor ikke muligt at genskabe data udfra en hash værdi. 

Det er muligt at udregne hash værdien på to måde, med en algoritme som bruger en kode eller en algoritme som ikke bruger en kode. 
Når der ikke bruges en kode til algoritmen, bruges dele af data som ønskes hash'et som nøglen.

[Microsoft doc's hashing](
https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm)

Følgende hashing algoritmer er tilgængelige:

```csharp
System.Security.Cryptography.HashAlgorithm
System.Security.Cryptography.KeyedHashAlgorithm
System.Security.Cryptography.MD5
System.Security.Cryptography.RIPEMD160
System.Security.Cryptography.SHA1
System.Security.Cryptography.SHA256
System.Security.Cryptography.SHA384
System.Security.Cryptography.SHA512
```

### Hashing (non-keyed)

Således bruges non-keyed hash

1. Lav et `HashAlgorithm` object

2. Konverter data som skal hash'es til et byte array

3. Kald `HashAlgorithm.ComputeHash()` med byte data som argument

4. Gem byte array'et som udgør hash værdien ved at kalde `HashAlgorithm.Hash` property

#### Eksempel (non-keyed)

```csharp
public static class NonKeyHash
{
    public static string Compute(string data, HashAlgorithm hash = null)
    {
        hash = hash ?? new MD5CryptoServiceProvider();

        hash.ComputeHash(Encoding.UTF8.GetBytes(data));

        return Convert.ToBase64String(hash.Hash);
    }
}
```

### Hashing (keyed)

Således bruges keyed hash

1. Lav et `KeyedHashAlgorithm` object

    Angiv nøglen ved kald til konstruktøren eller brug den nøgle som lave automatisk.

2. Konverter data som skal hash'es til et byte array

3. Kald `HashAlgorithm.ComputeHash()` med byte data som argument

4. Gem byte array'et som udgør hash værdien ved at kalde `HashAlgorithm.Hash` property

#### Eksempel (keyed)

```csharp
public static class KeyHash
{
    public static string Compute(string data, string key = null, KeyedHashAlgorithm hash = null)
    {
        hash = hash ??
            (key == null ? new HMACSHA1() : new HMACSHA1(Encoding.UTF8.GetBytes(key)));

        hash.ComputeHash(Encoding.UTF8.GetBytes(data));

        return Convert.ToBase64String(hash.Hash);
    }
}
```

## Digital Signatur (signering og validering af data)
Digital signaturer giver mulighed for at validere at data kommer fra en konkret afsender og ikke er blevet ændret undervejs.
Dette er ikke at forveksle med kryptering. En digital signatur a data bruges ofte i denne sammehæng, men kan sagtens bruges alene.

Digital signaturer bruger asymmetrisk kryptering (privat/offentligt nøgle par).

Således laves signeres data:

1. Afsender beregner en hash værdi for data.

2. Afsender kryptere hash værdien med sin private nøgle

3. Afsender sender besked (data) sammen med signatur = krypteret hash værdi.

### Eksempel (signering af data)

```csharp
public static DigitalSignaturData Sign(string text, RSACryptoServiceProvider algorithm = null)
{
    var signer = algorithm ?? new RSACryptoServiceProvider();
    byte[] data = Encoding.UTF8.GetBytes(text);
    byte[] signature = signer.SignData(data, new SHA1CryptoServiceProvider());

    return new DigitalSignaturData
    {
        PublicKey = signer.ToXmlString(false),
        Signatur = Convert.ToBase64String(signature),
        Text = text
    };
}
```

Således valideres signeret data:

1. Modtager beregner hash værdi udfra modtaget besked (data)

2. Modtager kan nu dekryptere hash værdien med afsender offentlige nøgle.

3. Modtager sammenligner hash værdierne, hvis de er en er data uændret.

### Eksempel (validering af signatur)

```csharp
public static bool Validate(DigitalSignaturData digitalSignaturData, RSACryptoServiceProvider algorithm = null)
{
    var verifier = algorithm ?? new RSACryptoServiceProvider();
    verifier.FromXmlString(digitalSignaturData.PublicKey);
    byte[] receivedData = Encoding.UTF8.GetBytes(digitalSignaturData.Text);

    return verifier.VerifyData(receivedData, new SHA1CryptoServiceProvider(), Convert.FromBase64String(digitalSignaturData.Signatur));
}
```

## Rettigheder

Kode kan benyttes frit men smid gerne et link retur til dette repository.
