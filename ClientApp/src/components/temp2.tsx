import { Button } from "@mui/material";
import { Link } from "react-router-dom";
import { useAppSelector } from "../store/hooks";
import React, { useState } from 'react'


export const Temp2 = () => {
    const [state, setState] = useState(false);
    const s = useAppSelector(state => state.authReduser.isAuthenticated);
    return (
        <>
            <Link to='/game'>game</Link>
            Hey hey storage state is {s ? <>true</> : <>false</>}
            <br />But local state is {state ? <>true</> : <>false</>}
            <Button
                onClick={() => setState(!state)}
            >Update
            </Button>
        </>
    )
}