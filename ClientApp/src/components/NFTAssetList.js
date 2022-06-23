import React, { Component } from 'react';
import Wallet from '../helpers/Wallet';
import { Row, Col, Card, CardHeader, CardBody } from 'reactstrap';

const FollowerCard = props => (
    <Card className="b text-center">
        <CardBody>
            <img className="rounded-circle thumb64 mb-2" src={props.imageUrl} />
            <p className="h4 text-bold mb-0">{props.tokenID}</p>
            <p>{props.collectionName}</p>
        </CardBody>
        <CardBody className="bt">
            <Row>
                <Col col="3" className="br">
                    <small>Purchase</small>
                    <br/>
                    <em className="fab fa-ethereum fa-fw text-primary"></em>
                    <br />
                    <strong>{props.purchasePrice}</strong>
                </Col>
                <Col col="3" className="br">
                    <small>Floor</small>
                    <br />
                    <em className="fab fa-ethereum fa-fw text-primary"></em>
                    <br />
                    <strong>{props.floorPrice}</strong>
                </Col>
                <Col xs="3" className="br">
                    <small>P&L</small>
                    <br />
                    <em className="fab fa-ethereum fa-fw text-primary"></em>
                    <br />
                    <strong>{props.profitLossAmount}</strong>
                </Col>
                <Col xs="3">
                    <small>P&L %</small>
                    <br />
                    <em className="fab fa-ethereum fa-fw text-primary"></em>
                    <br />
                    <strong>{props.profitLossAmount}</strong>
                </Col>
            </Row>
        </CardBody>
    </Card>
)

export default class NFTAssetList extends Component {
    static displayName = NFTAssetList.name;

    constructor(props) {
        super(props);
        this.state = { assets: {}, loading: true };

        //Function bindings to this
        this.prevPage = this.prevPage.bind(this);
        this.nextPage = this.nextPage.bind(this);
    }

    componentDidMount() {
        this.loadNFTAssetPage();
    }

    prevPage() {
        this.setState({ assets: {}, loading: true });
        this.loadNFTAssetPage(this.state.assets.prevPageCursor);
    }

    nextPage() {
        this.setState({ assets: {}, loading: true });
        this.loadNFTAssetPage(this.state.assets.nextPageCursor);
    }

    renderTransactionsTable(assets) {
        return (
            <Col xl="9" lg="8">
                <Row>
                    {assets.dataList.map(t =>
                        <Col key={t.transactionHash + t.tokenID} xl="4" lg="6">
                            <FollowerCard collectionName={t.collectionName}
                                tokenID={t.tokenID}
                                imageUrl={t.imageUrl}
                                purchasePrice={t.purchasePrice}
                                floorPrice={t.floorPrice}
                                profitLossAmount={t.profitLossAmount}
                                profitLossPercent={t.profitLossPercent} />
                        </Col>
                    )}
                </Row>
                <span>
                    <button disabled={assets.prevPageCursor == null} onClick={this.prevPage} >Previous</button>
                    <button disabled={assets.nextPageCursor == null} onClick={this.nextPage}>Next</button>
                </span>
            </Col>
        );
    }

    render() {
        var contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderTransactionsTable(this.state.assets);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async loadNFTAssetPage(pageCursor) {
        await Wallet.connect();

        if (Wallet.isConnected) {
            var params = new URLSearchParams({ accountAddress: Wallet.address, pageCursor: pageCursor == null ? '' : pageCursor });
            const response = await fetch('api/nftasset?' + params);
            const data = await response.json();
            console.log(data);
            this.setState({ assets: data, loading: false });
        } else {
            this.setState({ assets: {}, loading: false });
        }
    }
}
