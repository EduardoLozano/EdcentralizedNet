import React, { Component } from 'react';
import PropTypes from 'prop-types';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../store/actions/actions';

class Header extends Component {

    componentDidMount() {
    }

    resize () {
        // all IE friendly dispatchEvent
        var evt = document.createEvent('UIEvents');
        evt.initUIEvent('resize', true, false, window, 0);
        window.dispatchEvent(evt);
        // modern dispatchEvent way
        // window.dispatchEvent(new Event('resize'));
    }

    render() {
        return (
            <header className="topnavbar-wrapper">
                { /* START Top Navbar */ }
                <nav className="navbar topnavbar align-items-center justify-content-center">
                    { /* START navbar header */ }
                    <div className="navbar-header">
                        <a className="navbar-brand" href="#/">
                            <div className="brand-logo text-center">
                                <span className="" style={{ color: 'white' }}>EdcentralizedNet</span>
                            </div>
                        </a>
                    </div>
                    { /* END navbar header */ }
                </nav>
                { /* END Top Navbar */ }
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