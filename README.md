# neat-coin
A didactic crypto-currency based on blockchain, and built in TDD with .NET Core

## Steps

* A Ledger can host one Transaction. A Transaction contains a Sender, a Receiver (which are Accounts) and an Amount
* The Ledger can return an Account's Balance;
* The Ledger can contain more Transactions; the Account Balance takes into account all the Transactions;
* Transactions are grouped into Pages; each Page contains at least one Transaction;
* Pages are linked together;
* Pages are linked through their Hash; Pages with different Transactions or linked to different parent Pages have different Hash values;
* A Page is valid if its Hash matches the Difficulty (if its Hash begins with a number of 0s equal or greater than the Ledger parameter "Diffuculty");
* A Ledger is valid if and only if all the contained Pages are valid;
* A valid Page contains a reward Transaction to a given Account;
* A Page is valid if and only if it keeps all the Account Balances positive;
* A Page is valid only if its Transactions are valid; Transactions are always valid;
* Transactions are signed by their Sender's Private Key, and can be verified by the Sender's Public Key;
* A Transaction is valid only if the Sign is verified; 
* A Transaction from A to B is chained to a previous Transaction from someone to A;
* There is a Page 0 with a Reward Transaction no other Transactions;
* Pages are called Blocks; the Page 0 is called Genesis Block; the Ledger is called BlockChain.
