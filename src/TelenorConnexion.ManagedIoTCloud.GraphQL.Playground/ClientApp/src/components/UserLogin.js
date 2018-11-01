import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Grid, Row, Col, Form, InputGroup, FormControl, FormGroup, Glyphicon, HelpBlock } from 'react-bootstrap';
import { actionCreators } from '../store/UserLogin';
import AppPageHeader from './AppPageHeader';
import HostnameHelpBlock from './HostnameHelpBlock';

const UserLogin = props =>
  <div>
    <AppPageHeader />
    <Grid>
      <Form horizontal>
        <FormGroup controlId='hostname' validationState={props.validationState.hostname}>
          <Col smOffset={3} sm={6} componentClass={InputGroup}>
            <InputGroup.Addon><Glyphicon glyph='globe'/></InputGroup.Addon>
            <FormControl type='text' placeholder='Hostname' required onBlur={props.onHostnameChanged}/>
            <FormControl.Feedback/>
          </Col>
          {HostnameHelpBlock(props, 3, 6)}
        </FormGroup>
        <FormGroup controlId='username'>
          <Col smOffset={3} sm={6} componentClass={InputGroup}>
            <InputGroup.Addon><Glyphicon glyph='user'/></InputGroup.Addon>
            <FormControl type='text' placeholder='Username' required/>
            <FormControl.Feedback/>
          </Col>
        </FormGroup>
        <FormGroup controlId='password'>
          <Col smOffset={3} sm={6} componentClass={InputGroup}>
            <InputGroup.Addon><Glyphicon glyph='lock'/></InputGroup.Addon>
            <FormControl type='text' placeholder='Password' required/>
            <FormControl.Feedback/>
          </Col>
        </FormGroup>
      </Form>
    </Grid>
  </div>;

export default connect(
  state => state.userLogin,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(UserLogin);
