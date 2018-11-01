const HOSTNAME_CHANGED = 'HOSTNAME_CHANGED';
const USE_NEW_HOSTNAME_FAILED = 'USE_NEW_HOSTNAME_FAILED';
const FETCH_MIC_MANIFEST = 'FETCH_MIC_MANIFEST';
const RECEIVE_MIC_MANIFEST = 'RECEIVE_MIC_MANIFEST';
const API_ROOT_URL_CHANGED = 'API_ROOT_URL_CHANGED';
const FETCH_API_MANIFEST = 'FETCH_API_MANIFEST';
const RECEIVE_API_MANIFEST = 'RECEIVE_API_MANIFEST';
const API_KEY_CHANGED = 'API_KEY_CHANGED';

const initialState = {
  hostname: undefined,
  manifest: undefined,
  apiRootUrl: undefined,
  apiManifest: undefined,
  apiKey: undefined,
  validationState: {
    hostname: undefined
  },
  helpText: {
    hostname: undefined
  }
};

const fetchManifest = async (hostname, dispatch) => {
  dispatch({
    type: FETCH_MIC_MANIFEST,
    hostname
  });

  let qHostname = encodeURIComponent(hostname);
  try {
    let response = await fetch(`https://1u31fuekv5.execute-api.eu-west-1.amazonaws.com/prod/manifest/?hostname=${qHostname}`);
    let micManifest = await response.json();
    dispatch({
      type: RECEIVE_MIC_MANIFEST,
      manifest: micManifest
    });
    return micManifest;
  } catch (e) {
    dispatch({
      type: USE_NEW_HOSTNAME_FAILED,
      error: e
    });
    return undefined;
  }
};

const getApiRootUrl = (manifest, dispatch) => {
  try {
    let apiRootUrl = manifest.ApiGatewayRootUrl + '/';
    if (typeof manifest.StackName !== 'undefined' && manifest.StackName) {
      apiRootUrl += manifest.StackName + '/';
    }
    dispatch({
      type: API_ROOT_URL_CHANGED,
      apiRootUrl
    });
    return apiRootUrl;
  } catch (e) {
    dispatch({
      type: USE_NEW_HOSTNAME_FAILED,
      error: e
    });
    return undefined;
  }
};

const fetchApiManifest = async (apiRootUrl, dispatch) => {
  try {
    let apiManifestUrl = `${apiRootUrl}metadata/manifest`;
    dispatch({
      type: FETCH_API_MANIFEST,
      apiManifestUrl
    });
    let response = await fetch(apiManifestUrl);
    let apiManifest = await response.json();
    dispatch({
      type: RECEIVE_API_MANIFEST,
      manifest: apiManifest
    });
    return apiManifest;
  } catch (e) {
    dispatch({
      type: USE_NEW_HOSTNAME_FAILED,
      error: e
    });
    return undefined;
  }
};

const getApiKey = (manifest, dispatch) => {
  try {
    let apiKey = manifest.ApiKey;
    dispatch({
      type: API_KEY_CHANGED,
      apiKey
    });
    return apiKey;
  } catch (e) {
    dispatch({
      type: USE_NEW_HOSTNAME_FAILED,
      error: e
    });
    return undefined;
  }
};

const validateHostname = (hostname, apiRootUrl, apiKey) => {
  if (typeof hostname === 'undefined' || !hostname) {
    return null;
  }
  else if (typeof apiRootUrl === 'undefined' || !apiRootUrl) {
    return 'warning';
  }
  else if (typeof apiKey === 'undefined' || !apiKey) {
    return 'warning';
  }
  return 'success';
};

export const actionCreators = {
  onHostnameChanged: (event) => {
    let hostname = event.target.value;
    return async (dispatch) => {
      dispatch({
        type: HOSTNAME_CHANGED,
        hostname
      });
      if (!hostname) {
        return;
      }
      let micManifest = await fetchManifest(hostname, dispatch);
      if (typeof micManifest === 'undefined') {
        return;
      }
      let apiRootUrl = getApiRootUrl(micManifest, dispatch);
      if (typeof apiRootUrl === 'undefined') {
        return;
      }
      let apiManifest = await fetchApiManifest(apiRootUrl, dispatch);
      if (typeof apiManifest === 'undefined') {
        return;
      }
      let apiKey = getApiKey(apiManifest, dispatch);
    };
  }
};

export const reducer = (state, action) => {
  console.log('UserLogin state change', {
    currentState: state,
    action
  });
  state = state || initialState;
  if (action.type === HOSTNAME_CHANGED) {
    return {
      ...state,
      hostname: action.hostname,
      manifest: undefined,
      apiManifest: undefined,
      apiRootUrl: undefined,
      apiKey: undefined,
      validationState: {
        ...state.validationState,
        hostname: validateHostname(action.hostname, undefined, undefined)
      },
      helpText: {
        ...state.helpText,
        hostname: undefined
      }
    };
  } else if (action.type === FETCH_MIC_MANIFEST) {
    return {
      ...state,
      validationState: {
        ...state.validationState,
        hostname: validateHostname(action.hostname, undefined, undefined)
      },
      helpText: {
        ...state.helpText,
        hostname: 'Fetching MIC Stack manifest document. . .'
      }
    };
  } else if (action.type === RECEIVE_MIC_MANIFEST) {
    return {
      ...state,
      manifest: action.manifest,
      validationState: {
        ...state.validationState,
        hostname: validateHostname(action.hostname, undefined, undefined)
      },
      helpText: {
        ...state.helpText,
        hostname: 'Fetching MIC Stack manifest document. . . Succeeded!'
      }
    };
  } else if (action.type === API_ROOT_URL_CHANGED) {
    return {
      ...state,
      apiRootUrl: action.apiRootUrl,
      validationState: {
        ...state.validationState,
        hostname: validateHostname(state.hostname, action.apiRootUrl, undefined)
      }
    };
  } else if (action.type === FETCH_API_MANIFEST) {
    return {
      ...state,
      validationState: {
        ...state.validationState,
        hostname: validateHostname(state.hostname, state.apiRootUrl, undefined)
      },
      helpText: {
        ...state.helpText,
        hostname: 'Fetching MIC API manifest document. . .'
      }
    };
  } else if (action.type === RECEIVE_API_MANIFEST) {
    return {
      ...state,
      apiManifest: action.manifest,
      validationState: {
        ...state.validationState,
        hostname: validateHostname(state.hostname, state.apiRootUrl, undefined)
      },
      helpText: {
        ...state.helpText,
        hostname: 'Fetching MIC API manifest document. . . Succeeded!'
      }
    };
  } else if (action.type === API_KEY_CHANGED) {
    return {
      ...state,
      apiKey: action.apiKey,
      validationState: {
        ...state.validationState,
        hostname: validateHostname(state.hostname, state.apiRootUrl, action.apiKey)
      },
      helpText: {
        ...state.helpText,
        hostname: 'Connected to MIC Cloud REST API'
      }
    };
  } else if (action.type === USE_NEW_HOSTNAME_FAILED) {
    return {
      ...state,
      hostname: undefined,
      manifest: undefined,
      apiManifest: undefined,
      apiRootUrl: undefined,
      apiKey: undefined,
      validationState: {
        ...state.validationState,
        hostname: 'error'
      },
      helpText: {
        ...state.helpText,
        hostname: action.error
      }
    };
  }
  return state;
};
