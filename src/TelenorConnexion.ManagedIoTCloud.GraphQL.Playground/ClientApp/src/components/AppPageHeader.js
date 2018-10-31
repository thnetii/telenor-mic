import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { connect } from 'react-redux';

const AppPageHeader = () =>
  <div className="text-center">
    <PageHeader>
      Telenor Connexion Managed IoT Cloud
      <br/>
      <small>Interactive GraphQL Playground</small>
    </PageHeader>
  </div>;

export default connect()(AppPageHeader);
