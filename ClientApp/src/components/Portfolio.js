import React, { Component } from 'react';
import Wallet from '../helpers/Wallet';

export class Portfolio extends Component {
    static displayName = Portfolio.name;

    constructor(props) {
        super(props);
        this.state = { portfolio: {}, loading: true };
    }

    componentDidMount() {
        this.populateTransactionData();
    }

    static renderTransactionsTable(portfolio) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Collection Name</th>
                        <th>Token ID</th>
                        <th>Purchase Date</th>
                        <th>Purchase Price (ETH)</th>
                        <th>Floor Price (ETH)</th>
                        <th>P&L (ETH)</th>
                        <th>P&L %</th>
                    </tr>
                </thead>
                <tbody>
                    {portfolio.tokens.map(t =>
                        <tr key={t.transactionHash + t.tokenID}>
                            <td>{t.collectionName}</td>
                            <td>{t.tokenID}</td>
                            <td>{(new Date(t.purchaseDate)).toLocaleString()}</td>
                            <td>{t.purchasePrice} ETH</td>
                            <td>{t.floorPrice} ETH</td>
                            <td style={{ color: t.profitLossAmount < 0 ? "red" : "green" }}>{t.profitLossAmount} ETH</td>
                            <td style={{ color: t.profitLossAmount < 0 ? "red" : "green" }}>{t.profitLossPercent}%</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    static renderHeaderTable(portfolio) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Invested Value (ETH)</th>
                        <th>Profit Loss Amount (ETH)</th>
                        <th>Profit Loss Percent</th>
                    </tr>
                </thead>
                <tbody>
                    <tr key="headerKey">
                        <td>{portfolio.investedValue} ETH</td>
                        <td style={{ color: portfolio.profitLossAmount < 0 ? "red" : "green" }}>{portfolio.profitLossAmount} ETH</td>
                        <td style={{ color: portfolio.profitLossAmount < 0 ? "red" : "green" }}>{portfolio.profitLossPercent}%</td>
                    </tr>
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Portfolio.renderTransactionsTable(this.state.portfolio);

        let header = this.state.loading
            ? <p><em>Loading...</em></p>
            : Portfolio.renderHeaderTable(this.state.portfolio);

        return (
            <div>
                <h1 id="tabelLabel" >My NFT Portfolio</h1>
                {header}
                {contents}
            </div>
        );
    }

    async populateTransactionData() {
        await Wallet.connect();

        if (Wallet.isConnected) {
            var params = new URLSearchParams({ accountAddress: Wallet.address });
            const response = await fetch('portfolio?' + params);
            const data = await response.json();
            console.log(data);
            this.setState({ portfolio: data, loading: false });
        } else {
            this.setState({ portfolio: {}, loading: false });
        }
    }
}
