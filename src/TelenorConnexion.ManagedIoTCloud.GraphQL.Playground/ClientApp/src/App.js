import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import GraphqlPlayground from './components/GraphqlPlayground';

const App = () =>
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/counter' component={Counter} />
    <Route path='/playground' component={GraphqlPlayground} />
  </Layout>;

export default App;
