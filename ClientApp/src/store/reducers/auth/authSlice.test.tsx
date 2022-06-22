/**
 * @jest-environment jsdom
 */
 import { AnyAction } from 'redux'
import reducer, { authSlice, AuthState, signInComplete } from './authSlice'

test('should return the initial state', () => {
  
  expect(reducer(undefined, {} as AnyAction)).toEqual(
    {
      error: false, 
      isAuthenticated: false, 
      name: "",
      requestSended: true
    }
  )
})

test('should change to authenticate state', () => {
  const initialState: AuthState = {
    isAuthenticated: false,
    requestSended: true,
    error: false,
    name: ''
  };
  expect(reducer(initialState, signInComplete("name"))).toEqual(
    {
      isAuthenticated: true,
      requestSended: true,
      error: false,
      name: 'name'
    }
  );
})