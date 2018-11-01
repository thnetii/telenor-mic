import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import GraphqlPlayground from './components/GraphqlPlayground';
import UserLogin from './components/UserLogin';

const App = () =>
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/login' component={UserLogin} />
    <Route path='/counter' component={Counter} />
    <Route path='/playground' component={GraphqlPlayground} />
  </Layout>;

export default App;
