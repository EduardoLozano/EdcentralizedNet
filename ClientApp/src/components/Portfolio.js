import React, { Component } from 'react';
import Wallet from '../helpers/Wallet';

export class Portfolio extends Component {
    static displayName = Portfolio.name;

    constructor(props) {
        super(props);
        this.state = { portfolio: {}, loading: true };

        //Function bindings to this
        this.prevPage = this.prevPage.bind(this);
        this.nextPage = this.nextPage.bind(this);
    }

    componentDidMount() {
        this.populateTransactionData();
    }

    prevPage() {
        this.setState({ portfolio: {}, loading: true });
        this.populateTransactionData(this.state.portfolio.prevPageCursor);
    }

    nextPage() {
        this.setState({ portfolio: {}, loading: true });
        this.populateTransactionData(this.state.portfolio.nextPageCursor);
    }

    renderTransactionsTable(portfolio) {
        return (
            <div>
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th></th>
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
                                <td><img src={t.imageUrl}></img></td>
                                <td>{t.collectionName}</td>
                                <td style={{ wordBreak: "break-word" }}>{t.tokenID}</td>
                                <td>{(new Date(t.purchaseDate)).toLocaleString()}</td>
                                <td>{t.purchasePrice} ETH</td>
                                <td>{t.floorPrice} ETH</td>
                                <td style={{ color: t.profitLossAmount < 0 ? "red" : "green" }}>{t.profitLossAmount} ETH</td>
                                <td style={{ color: t.profitLossAmount < 0 ? "red" : "green" }}>{t.profitLossPercent}%</td>
                            </tr>
                        )}
                    </tbody>
                </table>
                <span>
                    <button disabled={portfolio.prevPageCursor == null} onClick={this.prevPage} >Previous</button>
                    <button disabled={portfolio.nextPageCursor == null} onClick={this.nextPage}>Next</button>
                </span>
            </div>
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
            : this.renderTransactionsTable(this.state.portfolio);

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

    async populateTransactionData(pageCursor) {
        await Wallet.connect();

        if (Wallet.isConnected) {
            var params = new URLSearchParams({ accountAddress: Wallet.address, pageCursor: pageCursor == null ? '' : pageCursor });
            const response = await fetch('api/portfolio?' + params);
            const data = await response.json();
            console.log(data);
            this.setState({ portfolio: data, loading: false });
        } else {
            this.setState({ portfolio: {}, loading: false });
        }
    }
}
