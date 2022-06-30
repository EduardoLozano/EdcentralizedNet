import React, { Component } from 'react';
import NFTAssetList from './NFTAssetList';
import AccountSummary from './AccountSummary';

export default class Portfolio extends Component {
    static displayName = Portfolio.name;

    componentDidMount() {
    }

    render() {
        return (
            <div>
                <AccountSummary/>
                <NFTAssetList/>
            </div>
        );
    }
}
