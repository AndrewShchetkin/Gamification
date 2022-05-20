import { List } from "@mui/material";
import React from "react";

interface ListProps<T> {
    items: T[];
    renderItem: (item: T) => React.ReactNode
}

export default function ReusedList<T>(props: ListProps<T>) {

    return (
        <List>
            {props.items.map(props.renderItem)}
        </List>
    )
}