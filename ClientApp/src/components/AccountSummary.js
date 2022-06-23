import React, { Component } from 'react';
import Wallet from '../helpers/Wallet';
import ContentWrapper from './Layout/ContentWrapper';
import { Row, Col } from 'reactstrap';

export default class AccountSummary extends Component {
    static displayName = AccountSummary.name;

    constructor(props) {
        super(props);
        this.state = { portfolio: {}, loading: true };
    }

    componentDidMount() {
        this.loadPortfolioInformation();
    }

    static renderHeaderTable(portfolio) {
        var profitLossSymbolClass = "col-4 d-flex align-items-center justify-content-center rounded-left ";
        var profitLossCardClass = "col-8 py-3 rounded-right ";

        if (portfolio.profitLossAmount < 0) {
            profitLossSymbolClass += "bg-danger-dark";
            profitLossCardClass += "bg-danger-light";
        } else {
            profitLossSymbolClass += "bg-success-dark";
            profitLossCardClass += "bg-success-light";
        }

        return (
            <ContentWrapper>
                <Row>
                    <Col xl={3} md={6}>
                        { /* START card */}
                        <div className="card flex-row align-items-center align-items-stretch border-0">
                            <div className="col-4 d-flex align-items-center bg-primary-dark justify-content-center rounded-left">
                                <em className="fa-3x fab fa-ethereum"></em>
                            </div>
                            <div className="col-8 py-3 bg-primary rounded-right">
                                <div className="h2 mt-0">{portfolio.investedValue} <small>ETH</small></div>
                                <div className="text-uppercase">Invested</div>
                            </div>
                        </div>
                    </Col>
                    <Col xl={3} md={6}>
                        { /* START card */}
                        <div className="card flex-row align-items-center align-items-stretch border-0">
                            <div className={profitLossSymbolClass}>
                                <em className="icon-globe fa-3x"></em>
                            </div>
                            <div className={profitLossCardClass}>
                                <div className="h2 mt-0">{portfolio.profitLossAmount} <small>ETH</small></div>
                                <div className="text-uppercase">Profit & Loss</div>
                            </div>
                        </div>
                    </Col>
                    <Col xl={3} lg={6} md={12}>
                        { /* START card */}
                        <div className="card flex-row align-items-center align-items-stretch border-0">
                            <div className={profitLossSymbolClass}>
                                <em className="icon-bubbles fa-3x"></em>
                            </div>
                            <div className={profitLossCardClass}>
                                <div className="h2 mt-0">{portfolio.profitLossPercent} <small>%</small></div>
                                <div className="text-uppercase">Profit & Loss %</div>
                            </div>
                        </div>
                    </Col>
                </Row>
            </ContentWrapper>
        );
    }

    render() {
        var contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : AccountSummary.renderHeaderTable(this.state.portfolio);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async loadPortfolioInformation(pageCursor) {
        await Wallet.connect();

        if (Wallet.isConnected) {
            var params = new URLSearchParams({ accountAddress: Wallet.address });
            const response = await fetch('api/portfolio?' + params);
            const data = await response.json();
            console.log(data);
            this.setState({ portfolio: data, loading: false });
        } else {
            this.setState({ portfolio: {}, loading: false });
        }
    }
}
