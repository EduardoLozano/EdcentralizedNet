import React, { Component } from 'react';
import Wallet from '../helpers/Wallet';
import ContentWrapper from './Layout/ContentWrapper';
import { Row, Col } from 'reactstrap';
import 'loaders.css/loaders.css';

export default class AccountSummary extends Component {
    static displayName = AccountSummary.name;

    constructor(props) {
        super(props);
        this.state = { portfolio: {}, loading: true };
    }

    componentDidMount() {
        this.loadPortfolioInformation();
    }

    static renderSummary(portfolio) {
        var profitLossSymbolClass = "col-4 d-flex align-items-center justify-content-center rounded-left ";
        var profitLossCardClass = "col-8 py-3 rounded-right ";
        var arrowSymbol = "fa-3x far ";

        if (portfolio.profitLossAmount < 0) {
            profitLossSymbolClass += "bg-danger-dark";
            profitLossCardClass += "bg-danger-light";
            arrowSymbol += "fa-arrow-alt-circle-down";
        } else {
            profitLossSymbolClass += "bg-success-dark";
            profitLossCardClass += "bg-success-light";
            arrowSymbol += "fa-arrow-alt-circle-up";
        }

        return (
            <ContentWrapper>
                <Row className="justify-content-md-center">
                    <Col xl={9} lg={8}>
                        <Row>
                            <Col xl={3} md={6}>
                                { /* START card */}
                                <div className="card flex-row align-items-center align-items-stretch border-0">
                                    <div className="col-4 d-flex align-items-center bg-primary-dark justify-content-center rounded-left">
                                        <em className="fa-3x fab fa-ethereum"></em>
                                    </div>
                                    <div className="col-8 py-3 bg-primary rounded-right">
                                        <div className="text-uppercase">Account Value</div>
                                        <div className="h2 mt-0">{portfolio.accountValue}</div>
                                    </div>
                                </div>
                            </Col>
                            <Col xl={3} md={6}>
                                { /* START card */}
                                <div className="card flex-row align-items-center align-items-stretch border-0">
                                    <div className="col-4 d-flex align-items-center bg-primary-dark justify-content-center rounded-left">
                                        <em className="fa-3x fab fa-ethereum"></em>
                                    </div>
                                    <div className="col-8 py-3 bg-primary rounded-right">
                                        <div className="text-uppercase">Initial Investment</div>
                                        <div className="h2 mt-0">{portfolio.investedValue}</div>
                                    </div>
                                </div>
                            </Col>
                            <Col xl={3} md={6}>
                                { /* START card */}
                                <div className="card flex-row align-items-center align-items-stretch border-0">
                                    <div className={profitLossSymbolClass}>
                                        <em className={arrowSymbol}></em>
                                    </div>
                                    <div className={profitLossCardClass}>
                                        <div className="text-uppercase">Profit & Loss</div>
                                        <div className="h2 mt-0">{portfolio.profitLossAmount} <small>ETH</small></div>
                                    </div>
                                </div>
                            </Col>
                            <Col xl={3} md={6}>
                                { /* START card */}
                                <div className="card flex-row align-items-center align-items-stretch border-0">
                                    <div className={profitLossSymbolClass}>
                                        <em className={arrowSymbol}></em>
                                    </div>
                                    <div className={profitLossCardClass}>
                                        <div className="text-uppercase">Profit & Loss %</div>
                                        <div className="h2 mt-0">{portfolio.profitLossPercent} <small>%</small></div>
                                    </div>
                                </div>
                            </Col>
                        </Row>
                    </Col>
                </Row>
            </ContentWrapper>
        );
    }

    static renderLoading() {
        return (
            <ContentWrapper>
                <Row className="justify-content-md-center">
                    <Col lg="9">
                        <div className="card card-default">
                            <div className="card-body loader-demo d-flex align-items-center justify-content-center">
                                <div className="pacman">
                                    <div></div>
                                    <div></div>
                                    <div></div>
                                    <div></div>
                                    <div></div>
                                </div>
                            </div>
                            <div className="card-body d-flex align-items-center justify-content-center">
                                <span>Summarizing all your NFT information</span>
                            </div>
                            <div className="card-body d-flex align-items-center justify-content-center">
                                <span>Initial load will take the longest so you wont have to wait later!</span>
                            </div>
                        </div>
                    </Col>
                </Row>
            </ContentWrapper>
        );
    }

    render() {
        var contents = this.state.loading
            ? AccountSummary.renderLoading()
            : AccountSummary.renderSummary(this.state.portfolio);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async loadPortfolioInformation(pageCursor) {
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
