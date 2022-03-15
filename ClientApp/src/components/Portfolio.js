import React, { Component } from 'react';
import Wallet from '../helpers/Wallet';

export class Portfolio extends Component {
    static displayName = Portfolio.name;

    constructor(props) {
        super(props);
        this.state = { transactions: [], loading: true };
    }

    componentDidMount() {
        this.populateTransactionData();
    }

    static renderTransactionsTable(transactions) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Token Name</th>
                        <th>Token ID</th>
                        <th>Transaction Date</th>
                        <th>Value (ETH)</th>
                    </tr>
                </thead>
                <tbody>
                    {transactions.map(t =>
                        <tr key={t.hash + t.tokenID}>
                            <td>{t.tokenName}</td>
                            <td>{t.tokenID}</td>
                            <td>{(new Date(t.timeStamp * 1000)).toLocaleString()}</td>
                            <td>{t.transaction.value/1000000000000000000} ETH</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Portfolio.renderTransactionsTable(this.state.transactions);

        return (
            <div>
                <h1 id="tabelLabel" >My NFT Portfolio</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }

    async populateTransactionData() {
        await Wallet.connect();

        if (Wallet.isConnected) {
            var params = new URLSearchParams({ accountAddress: Wallet.address });
            const response = await fetch('etherscan?' + params);
            const data = await response.json();
            console.log(data);
            this.setState({ transactions: data, loading: false });
        } else {
            this.setState({ transactions: [], loading: false });
        }
    }
}
