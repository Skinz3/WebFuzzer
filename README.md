# WebFuzzer
Un 'WebFuzzer' En C# .NET

![alt text](https://image.ibb.co/iM4VcH/vlcsnap_2018_04_02_16h32m03s566.png)

# Description

Ce programme permet de retrouver les dossiers / fichiers cachés sur un serveur. 
Le temps d'execution dépend du ping avec le serveur distant. (Avec wikipédia, environ 30ms)
Un algorithme de bruteforce teste toute les combinaisons possible avec 'n' caractères.
Il est possible de changer le nombre de caractère maximal, 'n' dans Form1.cs, ligne 33.

# Exemple

fuzzer.Start(2); avec un dictionnaire de {a,b,c} et une url : http://test.com testera les combinaisons:

http://test.com/a
http://test.com/b
http://test.com/c
http://test.com/aa
http://test.com/ab
http://test.com/ac
http://test.com/ba
http://test.com/bb
http://test.com/bc
http://test.com/ca
http://test.com/cc
http://test.com/cb.
