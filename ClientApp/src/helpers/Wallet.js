const Web3 = require('web3');

export default class Wallet {
    static isConnected = false;
    static address = '';

    static connect = async () => {
        if (this.isConnected && this.address != '') {
            return;
        }

        //Lets properly instantiate window.web3
        if (Web3.givenProvider != null) {
            window.web3 = new Web3(Web3.givenProvider);
        } else if (window.ethereum) {
            window.web3 = new Web3(window.ethereum);
        } else if (window.web3) {
            window.web3 = new Web3(window.web3.currentProvider);
        }

        //Reset propertied before attempting connection
        this.address = '';
        this.isConnected = false;

        //Try and get a connected account
        if (window.web3) {
            var accounts = await window.web3.eth.getAccounts();

            if (accounts.length > 0) {
                this.address = accounts[0];
                this.isConnected = true;
            } else {
                //Trigger connection approval
                await window.ethereum.enable();
                accounts = await window.web3.eth.getAccounts();

                //Try and get accounts again after approval
                if (accounts.length > 0) {
                    this.address = accounts[0];
                    this.isConnected = true;
                }
            }
        }

        //Handle accounts changing
        if (window.ethereum) {
            window.ethereum.on('accountsChanged', function (accounts) {
                this.address = accounts[0];
            });
        }
    }
}