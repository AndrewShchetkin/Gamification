import IQuiz from "../@types/AdminPanel/IQuiz";
import IQuizErrorMsg from "../@types/AdminPanel/IQuizErrorMsg";

export class Validator {

    static uploadFormValidate(quiz:IQuiz, filePath:string,
         setErrorMsgs:React.Dispatch<React.SetStateAction<IQuizErrorMsg>>,
          errorMsgs:IQuizErrorMsg): boolean {
        
        const dateBegin: number = Date.parse(quiz.dateBegin);
        const dateEnd: number = Date.parse(quiz.dateEnd);
        const fileExtension: string = filePath.split('.')[1];


        let success = true;
        const errors: IQuizErrorMsg = {name: '', dateBegin: '', dateEnd: '', filePath:'', serverError: errorMsgs.serverError};

        if (quiz.name == '') {
            success = false;
            errors.name = 'Название должно содержать хотя бы 1 символ'
        }
        if (dateBegin >= dateEnd) {
            success = false;
            errors.dateBegin = 'Дата начала < Дата окончания';
            errors.dateEnd = 'Дата окончания > Дата начала';
        }
        if (isNaN(dateBegin)) {
            success = false;
            errors.dateBegin = 'Выберите дату';
        }
        if (isNaN(dateEnd)) {
            success = false;
            errors.dateEnd = 'Выберите дату';
        }
        if (fileExtension !== 'xlsx' && fileExtension !== 'xls') {
            success = false;
            errors.filePath = 'Доступные форматы файла - .xlsx и .xls';
        }

        setErrorMsgs(errors);
        return success;

        
    }
}