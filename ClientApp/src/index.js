import 'core-js/es/string';
import 'core-js/es/array';
import 'core-js/es/map';
import 'core-js/es/set';
import 'core-js/es/object';
import 'core-js/es/promise';
import 'raf/polyfill';

import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import configureStore from './store/store';

const store = configureStore();
const rootElement = document.getElementById('root');

ReactDOM.render(<Provider store={store}>
                    <App />
                </Provider>
, rootElement);

registerServiceWorker();

//$(document).ready(async function () {
//    await Wallet.connect();

//    if (Wallet.isConnected) {
//        var params = new URLSearchParams({ accountAddress: Wallet.address });
//        const response = await fetch('etherscan?' + params);
//        const data = await response.json();
//        console.log(data);
//        const trxData = await web3.eth.getTransaction(data[0].hash);
//        console.log(trxData);
//    }
//});

//$.getJSON(testApiUri2, async function (data) {
//    console.log(data);
//});

//$.getJSON(testApiUri, async function (data) {
//    await Wallet.connect();

//    if (Wallet.isConnected) {
//        var contractABI = "";
//        contractABI = JSON.parse(data.result);

//        if (contractABI != '') {
//            var contractOptions = { from: Wallet.address };
//            var MyContract = new web3.eth.Contract(contractABI, "0xfb6916095ca1df60bb79ce92ce3ea74c37c5d359", contractOptions);
//            MyContract.defaultAccount = Wallet.address;

//            var test = await MyContract.methods.memberId("0xfe8ad7dd2f564a877cc23feea6c0a9cc2e783715").call();
//            var test2 = await MyContract.methods.members("1").call();
//            console.log(test);
//            console.log(test2);
//        } else {
//            console.log("Error");
//        }
//    }
//});