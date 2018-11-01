import React from 'react';
import { Col, HelpBlock } from 'react-bootstrap';

const HostnameHelpBlock = (props, smOffset, smWidth) => {
  if (typeof props !== 'object') {
    return;
  }
  let helpTextContainer = props.helpText;
  if (typeof helpTextContainer !== 'object') {
    return;
  }
  let helpText = helpTextContainer.hostname;
  if (typeof helpText === 'undefined') {
    return;
  }
  return <Col smOffset={smOffset} sm={smWidth} componentClass={HelpBlock}>{helpText}</Col>;
};

export default HostnameHelpBlock;
