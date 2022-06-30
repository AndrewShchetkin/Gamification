import IQuiz from "../@types/AdminPanel/IQuiz";

export interface IQuizErrorMsg {
    name: string,
    dateBegin: string,
    dateEnd: string,
    filePath: string
}

export class Validator {

    uploadFormValidate(quiz:IQuiz, filePath:string, setErrorMsgs:any): boolean {
        
        const dateBegin: number = Date.parse(quiz.dateBegin);
        const dateEnd: number = Date.parse(quiz.dateEnd);
        const fileExtension: string = filePath.split('.')[1];

        let success = true;
        const errorMsgs: IQuizErrorMsg = {name: '', dateBegin: '', dateEnd: '', filePath:''};

        if (quiz.name == '') {
            success = false;
            errorMsgs.name = 'Название должно содержать хотя бы 1 символ'
        }
        if (dateBegin >= dateEnd) {
            success = false;
            errorMsgs.dateBegin = 'Дата начала < Дата окончания';
            errorMsgs.dateEnd = 'Дата окончания > Дата начала';
        }
        if (isNaN(dateBegin)) {
            success = false;
            errorMsgs.dateBegin = 'Выберите дату';
        }
        if (isNaN(dateEnd)) {
            success = false;
            errorMsgs.dateEnd = 'Выберите дату';
        }
        if (fileExtension !== 'xlsx' && fileExtension !== 'xls') {
            success = false;
            errorMsgs.filePath = 'Доступные форматы файла - .xlsx и .xls';
        }

        setErrorMsgs(errorMsgs);
        return success;

        
    }
}