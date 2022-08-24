import React, { Component } from 'react';
import Wallet from '../helpers/Wallet';
import ContentWrapper from './Layout/ContentWrapper';
import { Row, Col } from 'reactstrap';
import 'loaders.css/loaders.css';
import NFTAssetList from './NFTAssetList';
import AccountSummary from './AccountSummary';

export default class Portfolio extends Component {
    static displayName = Portfolio.name;

    constructor(props) {
        super(props);
        this.state = { status: {} };
    }

    componentDidMount() {
        this.loadAccountStatus();
    }

    static renderPortfolio() {
        return (
            <div>
                <AccountSummary />
                <NFTAssetList />
            </div>
        );
    }

    static renderStatus(status) {
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
                                <span>{status.message}</span>
                            </div>
                        </div>
                    </Col>
                </Row>
            </ContentWrapper>
        );
    }

    render() {
        var contents = this.state.status.isLoaded
            ? Portfolio.renderPortfolio()
            : Portfolio.renderStatus(this.state.status);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async loadAccountStatus(pageCursor) {
        if (Wallet.isConnected) {
            var params = new URLSearchParams({ accountAddress: Wallet.address });
            const response = await fetch('api/accountstatus?' + params);
            const data = await response.json();
            console.log(data);
            this.setState({ status: data });
        } else {
            this.setState({ status: {} });
        }
    }
}
