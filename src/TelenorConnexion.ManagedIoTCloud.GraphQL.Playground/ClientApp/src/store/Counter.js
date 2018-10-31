const incrementCountType = 'INCREMENT_COUNT';
const resetCountType = 'RESET_COUNT';
const decrementCountType = 'DECREMENT_COUNT';
const initialState = { count: 0 };

export const actionCreators = {
  increment: () => ({ type: incrementCountType }),
  reset: () => ({ type: resetCountType }),
  decrement: () => ({ type: decrementCountType })
};

export const reducer = (state, action) => {
  state = state || initialState;

  if (action.type === incrementCountType) {
    return { ...state, count: state.count + 1 };
  }

  if (action.type === resetCountType) {
    return { ...state, count: initialState.count };
  }

  if (action.type === decrementCountType) {
    return { ...state, count: state.count - 1 };
  }

  return state;
};
