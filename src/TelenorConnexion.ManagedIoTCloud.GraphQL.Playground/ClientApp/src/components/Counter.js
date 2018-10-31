import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Button, ButtonGroup } from 'react-bootstrap';
import { actionCreators } from '../store/Counter';
import { Button } from 'react-bootstrap';

const Counter = props =>
  <div>
    <h1>Counter</h1>

    <p>This is an extended Counter example of a React component.</p>

    <p>Current count: <strong>{props.count}</strong></p>

    <ButtonGroup>
      <Button onClick={props.increment}>Increment</Button>
      <Button onClick={props.reset}>Reset</Button>
      <Button onClick={props.decrement}>Decrement</Button>
    </ButtonGroup>
  </div>;

export default connect(
  state => state.counter,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Counter);
