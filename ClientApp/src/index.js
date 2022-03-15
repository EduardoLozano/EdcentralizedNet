import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import $ from "jquery";
import Wallet from './helpers/Wallet'

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

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

ReactDOM.render(
    <BrowserRouter basename={baseUrl}>
        <App />
    </BrowserRouter>,
    rootElement);

registerServiceWorker();