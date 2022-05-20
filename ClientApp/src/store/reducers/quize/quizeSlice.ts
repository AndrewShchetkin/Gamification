import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit'
import { useAppDispatch } from '../../hooks';
import { QuizeState } from '../../../@types/ReduxTypes/quizeState';

export const initialState: QuizeState = {
    questionsIsLoaded: false,
    requestSended: false,
    currentQuestionIndex: 0,
    questions: [{text: 'First question', answers: []},
    {text: 'Second question', answers: []}] // TODO: remove after adding get question method
}

export const getQuestions = createAsyncThunk('get/api/user', async (): Promise<void> => {
    const dispatch = useAppDispatch();

    dispatch(startLoadData());
    try {

        // const response = await fetch('api/user');
        
        // console.log(response);
        // if (response.ok && response.body) {
        //     const body = await response.json();
        setTimeout(() => dispatch(addQuestions("")), 10000);
    // }
        
    } catch (ex) {
        console.error(ex);
    } 
    dispatch(endLoadData());
});

export const quizeSlice = createSlice({
    name: 'quize',
    initialState,
    reducers: {// функции которые меняют состояние 
        nextQuestion: state => {
            if (state.questions.length > state.currentQuestionIndex + 1 ) {
                state.currentQuestionIndex+=1;
                return;
            }
            alert('Questions are over');
        },
        startLoadData: state => {
            state.requestSended = true
        },
        endLoadData: state => {
            state.requestSended = false
        },
        addQuestions: (state: QuizeState, action: PayloadAction<any>)=>{
            state.questionsIsLoaded = true;
            console.log(action);
        }
        // TODO: add get questions method
    },
    extraReducers: (builder) => {
        builder.addCase(getQuestions.fulfilled, (state, action) => {
            state.requestSended = false;
        });
    }
});

export const { nextQuestion, startLoadData, endLoadData, addQuestions } = quizeSlice.actions;

export default quizeSlice.reducer;



