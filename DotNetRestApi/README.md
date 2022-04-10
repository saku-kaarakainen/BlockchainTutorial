# What is this project?
I guess I would say this is my proof that I know, atleast the very basics of a blockchain architecture.
This is just a proof of concept. There would be lot's of things to make it proper one.

# How to use this
You can read the swagger API documentation in order to the endpoint's function.

## Basics
1. Add new transactions. When you add a transaction, it will be added the next mined block. 
So you won't see your transaction on chain until it's mined. 
As you can also see, it doesn't have any security features. A proper blockchain would have public/private key and an enpoint to verify the transactions.

2. Mine blocks. The mining is done manually. 
A real version could do it automatically, for example every 10 minutes.

3. Get on chain data. - I guess you could call this the public ledger.

4. Node functions - With these endpoints you can add your node in the network.


# TODO-list
If I ever go back to develop this, I would implement following things:
 - Security: Increase the mining difficulty. Bitcoin does it automatically, it increases it every 2016 block.
 - Security: Create more nodes, you can do 51% attack really easy on this implementation:
     Just make bigger chain mine, and then register it to the network.
 - Automate network synchronization
 - Coinbase: A personal wallet. 
 It should have the public key and the private key, like a proper crypto wallet does.
 - Client app: The app which you could use to send the transactions
 - Smart contracts: Bitcoin doesn't have those, but ethereum does.
 - And much more...