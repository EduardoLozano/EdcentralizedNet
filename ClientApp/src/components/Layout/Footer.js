import React, { Component } from 'react';

class Footer extends Component {

    render() {
        const year = new Date().getFullYear()
        return (
            <footer className="footer-container align-items-center justify-content-center" style={{ marginLeft: "auto" }}>
                <div className="text-center">
                    <span>&copy; {year} - NFT Peek</span>
                </div>
            </footer>
        );
    }

}

export default Footer;
