# Cryptography-DotnetCore

## Overview:

This project was implemented within the scope of a third-year undergraduate course concerning secure programming techniques and concepts relating to the security of web applications. "Console version" directory contains a solution file developed in the .Net Core environment in C#. Within it, a number of widely used cryptographic algorithms are re-implemented the hard way, i.e without the use of existing crypto libraries and data types such as BigInteger.

At every point user input is thoroughly validated after which it is treated as UTF-8 encoded string converted into a byte array, which allows for the ecnryption and decryption of virtually any input type. Some of the algorithms that were implemented are:

* **Caesar** encryption/decription
* **Vigenere** encryption/decription
* **Diffe-Hellmann** key generation
* **Rsa** encryption/decription
* **Fermat's Little Theorem** and **Binary Exponentiation** For Rsa And Diffe-Hellmann key exchange

in addition to a few dozen help functions that handle encoding/decoding, validation and more.

## Web Application:

The second directory contains a solution file wherein the initial console application is re-implemented as an MVC web application, support for user accounts, session validation, proper data storage is added and path traversal attacks, through which users could gain access to other users' data, are prevented.