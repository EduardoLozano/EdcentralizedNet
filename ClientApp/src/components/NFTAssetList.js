import React, { Component } from 'react';
import Wallet from '../helpers/Wallet';
import { Row, Col, Card, CardHeader, CardBody, Button } from 'reactstrap';

const NFTAssetCard = props => (
    <Card className="b text-center">
        <CardHeader className="text-white bg-dark">{props.collectionName}</CardHeader>
        <CardBody>
            <Row>
                <Col col="6" className="text-center">
                    <img className="rounded-circle thumb96 mb-2" src={props.imageUrl} alt="" />
                    <p className="h4 text-bold mb-0 text-truncate">{props.tokenID}</p>
                </Col>
                <Col col="6" className="text-left">
                    <h3 style={{ color: props.profitLossAmount < 0 ? "red" : "green" }}>{props.profitLossPercent} %</h3>
                    <ul className="list-unstyled mb-0">
                        <li className="mb-1">
                            Cost: <em className="fab fa-ethereum fa-fw text-primary" style={{ marginLeft: "4px" }}></em>{props.purchasePrice}
                        </li>
                        <li className="mb-1">
                            Floor: <em className="fab fa-ethereum fa-fw text-primary"></em>{props.floorPrice}
                        </li>
                        <li className="mb-1">
                            P&L: <em className="fab fa-ethereum fa-fw text-primary" style={{ marginLeft: "8px" }}></em>{props.profitLossAmount}
                        </li>
                    </ul>
                </Col>
            </Row>
        </CardBody>
        <CardBody>
            <Row>
                <Col col="3">
                    <button className="btn btn-secondary"><a href={props.openseaUrl} target="_blank" rel="noopener noreferrer"><img src="OpenSea-Full-Logo-Dark.png" style={{ width: "30%" }} alt="OpenSea Link"></img></a></button>
                </Col>
            </Row>
        </CardBody>
    </Card>
)

export default class NFTAssetList extends Component {
    static displayName = NFTAssetList.name;

    constructor(props) {
        super(props);
        this.state = { assets: { dataList: [] }, loading: true };

        //Function bindings to this
        this.prevPage = this.prevPage.bind(this);
        this.nextPage = this.nextPage.bind(this);
    }

    componentDidMount() {
        this.loadNFTAssetPage(1);
    }

    prevPage() {
        this.setState({ loading: true });
        this.loadNFTAssetPage(this.state.pageNumber - 1, this.state.assets.prevPageCursor);
    }

    nextPage() {
        this.setState({ loading: true });
        this.loadNFTAssetPage(this.state.pageNumber + 1, this.state.assets.nextPageCursor);
    }

    static renderLoading() {
        return (
            <Row className="justify-content-md-center">
                <Col lg="9">
                    <div className="card card-default">
                        <div className="card-body loader-demo d-flex align-items-center justify-content-center">
                            <div className="ball-triangle-path">
                                <div></div>
                                <div></div>
                                <div></div>
                            </div>
                        </div>
                        <div className="card-body d-flex align-items-center justify-content-center">
                            <span>Getting fresh NFT data for you!</span>
                        </div>
                    </div>
                </Col>
            </Row>
        );
    }

    renderTransactionsTable(assets) {
        return (
            <Row className="justify-content-md-center">
                <Col xl="9" lg="8">
                    <Row>
                        {assets.dataList.map(t =>
                            <Col key={t.transactionHash + t.tokenID} xl="3" lg="6">
                                <NFTAssetCard collectionName={t.collectionName}
                                    tokenID={t.tokenID}
                                    imageUrl={t.imageUrl}
                                    openseaUrl={t.openseaUrl}
                                    purchasePrice={t.purchasePrice}
                                    floorPrice={t.floorPrice}
                                    profitLossAmount={t.profitLossAmount}
                                    profitLossPercent={t.profitLossPercent} />
                            </Col>
                        )}
                    </Row>
                    <Row className="justify-content-md-center">
                        <Button color="secondary" className="btn-labeled" disabled={assets.prevPageCursor == null} onClick={this.prevPage}>
                            <span className="btn-label"><i className="fa fa-arrow-left"></i></span> Previous
                        </Button>
                        <Button color="secondary" className="btn-labeled" disabled={assets.nextPageCursor == null} onClick={this.nextPage}>
                            Next
                            <span className="btn-label btn-label-right"><i className="fa fa-arrow-right"></i></span>
                        </Button>
                    </Row>
                </Col>
            </Row>
        );
    }

    render() {
        var contents = this.state.loading
            ? NFTAssetList.renderLoading()
            : this.renderTransactionsTable(this.state.assets);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async loadNFTAssetPage(pageNumber, pageCursor) {
        this.setState({ assets: { dataList: [] } });

        if (Wallet.isConnected) {
            var params = new URLSearchParams({ accountAddress: Wallet.address, pageNumber: pageNumber, pageCursor: pageCursor == null ? '' : pageCursor });
            const response = await fetch('api/nftasset?' + params);
            const data = await response.json();
            //console.log(data);
            this.setState({ assets: data, pageNumber: pageNumber });
        }


        this.setState({ loading: false });
    }
}
