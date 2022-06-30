import React, { Component } from 'react';
import { BrowserRouter } from 'react-router-dom';
import Wallet from './helpers/Wallet';

// App Routes
import Routes from './Routes';
// Vendor dependencies
import "./Vendor";
// Application Styles
import './styles/bootstrap.scss';
import './styles/app.scss'

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = { walletIsConnected: false };
    }

    async componentDidMount() {
        await Wallet.connect(() => this.updateWalletState());
        this.updateWalletState();
    }

    updateWalletState() {
        if (Wallet.isConnected) {
            this.setState({ walletIsConnected: true });
        } else {
            this.setState({ walletIsConnected: false });
        }
    }

    render() {
        // specify base href from env varible 'PUBLIC_URL'
        // use only if application isn't served from the root
        // for development it is forced to root only
        /* global PUBLIC_URL */
        const basename = process.env.NODE_ENV === 'development' ? '/' : (PUBLIC_URL || '/');

        return (
            <BrowserRouter basename={basename}>
                <Routes />
            </BrowserRouter>
        );
    }
}
