import React from 'react';
import { connect } from 'react-redux';
import AppPageHeader from './AppPageHeader';

const Home = props =>
  <div>
    <AppPageHeader />
  </div>;

export default connect()(Home);
