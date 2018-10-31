import React from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import NavMenu from './NavMenu';

export default props => (
  <Grid fluid height="100%" width="100%">
    <Row>
      {props.children}
    </Row>
    <NavMenu />
  </Grid>
);
