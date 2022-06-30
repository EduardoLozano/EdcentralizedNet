const Web3 = require('web3');

export default class Wallet {
    static isConnected = false;
    static address = '';

    static connect = async (onChangeCallback) => {
        var self = this;

        if (!self.isConnected || self.address == '') {
            //Lets properly instantiate window.web3
            if (Web3.givenProvider != null) {
                window.web3 = new Web3(Web3.givenProvider);
            } else if (window.ethereum) {
                window.web3 = new Web3(window.ethereum);
            } else if (window.web3) {
                window.web3 = new Web3(window.web3.currentProvider);
            }

            //Reset propertied before attempting connection
            self.address = '';
            self.isConnected = false;

            //Try and get a connected account
            if (window.web3) {
                try {
                    var accounts = await window.web3.eth.requestAccounts();

                    if (accounts.length > 0) {
                        self.address = accounts[0];
                        self.isConnected = true;
                    }
                } catch (error) {
                    if (error.code == 4001) {
                        //User rejected metamask request
                        self.address = "";
                        self.isConnected = false;
                    } else if (error.code == -32002) {
                        //Already requested accounts and still waiting
                    }
                }
            }

            //Handle accounts changing
            if (window.ethereum) {
                window.ethereum.on('accountsChanged', function (accounts) {
                    self.address = accounts.length > 0 ? accounts[0] : '';

                    if (self.address != '') {
                        self.isConnected = true;
                    } else {
                        self.isConnected = false;
                    }

                    if (onChangeCallback && typeof (onChangeCallback) == "function") {
                        onChangeCallback();
                    }
                });
            }
        }
    }

    static disconnect = async () => {
        //Mimic a disconnect of wallet
        //The only true way currently for the user to disconnect will be through their metamask
        var self = this;
        self.isConnected = false;
        self.address = false;
    }
}