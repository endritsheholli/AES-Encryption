# AES Encryption

This project was made for assignment 1 - Development of AES text encryption program in ASP.NET.

## Functionality

In this application is implemented AES (Advanced Encryption Standard) encryption algorithm. The two main functions that have been developed are text encryption and decryption.

Encryption has three possibilities:

- Only plaintext is given to be encrypted - In this case an encryption key and an initialization vector are generated  by default. Key which is used in encryption is generated as SHA-256 hash of given key and iv is generated as MD5 hash of given iv. The given key and IV are shown to the user.

- Plaintext and encryption key are given - Key SHA-256 hash is generated so that for any type of input the key used in the algorithm has a fixed length acceptable for AES. An initialization vector is generated and its MD5 hash is attached to the encryption function. The default IV is shown to the user.

- Plaintext, key and initialization vector are given - Key SHA-256 hash and IV MD5 hash are generated so that for any key and IV input are used their acceptable lengths for AES.

Decryption can have three cases:

- Ciphertext, key and initialization vector are given

  - If the key and the initialization vector are the same as those used to encrypt the corresponding text, the decryption is performed successfully.
  - If the key or the initialization vector, or both are not the same as those used to encrypt the corresponding text, decryption cannot be performed.

- Ciphertext and key are given - In the absence of the initialization vector decryption can not be performed.

- Only ciphertext is given - In the absence of the key and the initialization vector decryption can not be performed.

Used technology

- ASP.NET and Web Development

- .NET Desktop Framework

Members

[Genti Sheholli]

[Endrit Sheholli]
