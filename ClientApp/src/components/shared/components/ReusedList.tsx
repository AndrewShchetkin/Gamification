import React from "react";

interface ListProps<T> {
    items: T[] | undefined;
    renderItem: (item: T) => React.ReactNode
}

export default function ReusedList<T>(props: ListProps<T>) {

    return (
        <div>
            {props?.items?.map(props.renderItem)}
        </div>
    )
}