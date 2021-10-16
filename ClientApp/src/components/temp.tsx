import { Button } from '@mui/material'
import React, { useState } from 'react'
import { Link } from 'react-router-dom';
import { useAppSelctor } from '../app/hooks';

export const Temp = () => {
    const [state, setState] = useState(false);
    const s = useAppSelctor(state => state.auth.isAuthenticated);
    return (
        <>
            <Link to='/game2'>game2</Link>
            Hey hey storage state is {s ? <>true</> : <>false</>}
            <br />But local state is {state ? <>true</> : <>false</>}
            <Button
                onClick={() => setState(!state)}
            >Update
            </Button>
        </>
    )
}
