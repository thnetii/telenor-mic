import React from 'react';
import { Provider, connect } from 'react-redux';
import { Playground, store } from 'graphql-playground-react';
import { Session } from 'graphql-playground-react/lib/state/sessions/reducers';
import { defaultLinkCreator } from 'graphql-playground-react/lib/state/sessions/fetchingSagas';
import { HttpLink } from 'apollo-link-http';

/**
 * @param {Session} session Current session
 * @param {string?} wsEndpoint WebSocket endpoint
 * @returns {HttpLink} the link to GraphQL server
 */
function graphqlApolloLink(session, wsEndpoint) {

  let { headers } = session;

  if (typeof headers !== 'object') {
    headers = {};
  }
  for (let key in headers) {
    if (key.toUpperCase() === 'X-APOLLO-TRACING') {
      delete headers[key];
      break;
    }
  }
  session.headers = headers;

  console.log('graphqlApolloLink', session);
  let apolloLink =  defaultLinkCreator(session, wsEndpoint);
  console.log('defaultLinkCreator', apolloLink);
  return apolloLink;
}

const GraphqlPlayground = () =>
  <Provider store={store}>
    <Playground
        endpoint="https://qvx6ay1eog.execute-api.eu-west-1.amazonaws.com/prod/graphql"
        createApolloLink={graphqlApolloLink}
    />
  </Provider>;

export default connect()(GraphqlPlayground);
