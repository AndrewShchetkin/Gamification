import classes from "./input.module.scss"

interface InputProps {
    label?: string,
    placeholder?: string,
    name: string,
    id?: string,
    type?: React.HTMLInputTypeAttribute,
}

export default function Input(props: InputProps) {

    return (
        <label className={classes.customLabel}>
            {props.label}
            <input
                required
                id={props.id ?? props.name}
                name={props.name}
                type={props.type}
                placeholder={props.placeholder}
            />

        </label>
    );
}