import React from 'react';

const ConnectWalletMessage = props => (
    <div className="abs-center" style={{ position: 'relative' }}>
        <div className="text-center my-3">
            <h1 className="mb-3">
                <sup>
                    <em className="fab fa-ethereum fa-2x text-muted fa-spin text-info"></em>
                </sup>
                <em className="fab fa-ethereum fa-4x text-muted fa-spin text-purple"></em>
                <em className="fab fa-ethereum fa-lg text-muted fa-spin text-success"></em>
            </h1>
            <div className="text-bold text-lg mb-3">PLEASE CONNECT WALLET</div>
            <p className="lead m-0">Our request is only to read NFT information</p>
            <p className="lead m-0">We will not have access to process any transaction on your behalf</p>
        </div>
    </div>
)

export default ConnectWalletMessage;