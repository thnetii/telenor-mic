import React from 'react';
import { Grid } from 'react-bootstrap';
import NavMenu from './NavMenu';

const AppLayout = props =>
  <Grid fluid>
    {props.children}
    <NavMenu />
  </Grid>;

export default AppLayout;
