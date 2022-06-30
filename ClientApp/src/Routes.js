import React, { Suspense, lazy } from 'react';
import { withRouter, Switch, Route, Redirect } from 'react-router-dom';
import { TransitionGroup, CSSTransition } from 'react-transition-group';
import Wallet from './helpers/Wallet';

/* loader component for Suspense*/
import PageLoader from './components/Common/PageLoader';

/* Base pages */
import Base from './components/Layout/Base';

/* Used to render a lazy component with react-router */
const waitFor = Tag => props => <Tag {...props} />;
const Portfolio = lazy(() => import('./components/Portfolio'));
const ConnectWalletMessage = lazy(() => import('./components/ConnectWalletMessage'));

const Routes = ({ location }) => {
    location.pathname = Wallet.isConnected ? "/portfolio" : "/connectWallet";
    const currentKey = location.pathname.split('/')[1] || '/';
    const timeout = { enter: 500, exit: 500 };

    // Animations supported
    //      'rag-fadeIn'
    //      'rag-fadeInRight'
    //      'rag-fadeInLeft'

    const animationName = 'rag-fadeIn'

    return (<Base>
                <TransitionGroup>
                    <CSSTransition key={currentKey} timeout={timeout} classNames={animationName} exit={false}>
                        <div>
                            <Suspense fallback={<PageLoader />}>
                                <Switch location={location}>
                                    <Route path='/portfolio' component={waitFor(Portfolio)} />
                                    <Route path="/connectWallet" component={waitFor(ConnectWalletMessage)} />

                                    <Redirect to="/connectWallet" />
                                </Switch>
                            </Suspense>
                        </div>
                    </CSSTransition>
                </TransitionGroup>
            </Base>
    )
}

export default withRouter(Routes);