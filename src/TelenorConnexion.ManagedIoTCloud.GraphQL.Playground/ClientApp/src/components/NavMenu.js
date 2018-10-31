import React from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

const AppNavBar =  props =>
  <Navbar inverse fixedBottom fluid collapseOnSelect>
    <Navbar.Header>
      <Navbar.Brand>
        <Link to={'/'}>MIC GraphQL Playground</Link>
      </Navbar.Brand>
      <Navbar.Toggle />
    </Navbar.Header>
    <Navbar.Collapse>
      <Nav>
        <LinkContainer to={'/playground'}>
          <NavItem>
            <Glyphicon glyph='search' /> Playground
          </NavItem>
        </LinkContainer>
      </Nav>
      <Nav pullRight>
        <Navbar.Text>
          User <Glyphicon glyph='user'/>
        </Navbar.Text>
      </Nav>
    </Navbar.Collapse>
  </Navbar>;

export default AppNavBar;
