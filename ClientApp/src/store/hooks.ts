import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux'
import { RootState, AppDispatch } from './store'

export const useAppDispatch = () => useDispatch<AppDispatch>(); // функция меняет текущее состояние.  
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector; // Получение текущего состояния приложения (от редакса)