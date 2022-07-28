import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Wallet from '../../helpers/Wallet';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../store/actions/actions';

class Header extends Component {

    constructor(props) {
        super(props);
        this.state = { connectedAs: "" };
    }

    async componentDidMount() {
        await Wallet.connect(() => this.updateWalletState());
        this.updateWalletState();
    }

    updateWalletState() {
        if (Wallet.isConnected) {
            var connectedAs = Wallet.address.substr(0, 5);
            connectedAs += "...";
            connectedAs += Wallet.address.substr(Wallet.address.length - 4, Wallet.address.length - 1);
            this.setState({ connectedAs: connectedAs });
        } else {
            this.setState({ connectedAs: "" });
        }
    }

    signOutWallet() {
        alert("Signing out");
    }

    render() {

        var connectedAsLabel = <div></div>;

        if (this.state.connectedAs != "") {
            connectedAsLabel = <ul className="navbar-nav flex-row">
                <li className="nav-item">
                    <div className="mr-3" style={{ color: 'white' }}>
                        <em className="fa-lg fas fa-user-astronaut mr-2"></em>
                        Connected as: {this.state.connectedAs}
                    </div>
                </li>
            </ul>;
        }

        return (
            <header className="topnavbar-wrapper">
                <nav className="navbar topnavbar">
                    <div className="navbar-header">
                        <div className="navbar-brand brand-logo">
                            <span className="" style={{ color: 'white' }}>NFT Peek</span>
                        </div>
                    </div>
                    {connectedAsLabel}
                </nav>
            </header>
        );
    }

}

Header.propTypes = {
    actions: PropTypes.object,
    settings: PropTypes.object
};

const mapStateToProps = state => ({ settings: state.settings })
const mapDispatchToProps = dispatch => ({ actions: bindActionCreators(actions, dispatch) })

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Header);