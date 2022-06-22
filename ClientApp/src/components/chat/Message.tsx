import React from 'react'
import classes from './Chat.module.css';

interface Props {
    message: string,
    userName: string
}

export function MessageLeft(props: Props) {
    const {message, userName} = props

    return (
        <div className={classes.messageLeftWrapper}>
            <div className={classes.messageLeftContent}>
                <div className={classes.messageLeftAuthor}>{userName + ":" }</div>
                <div className={classes.messageLeftText}>
                    <p>{message}</p>
                </div>
            </div>
        </div>
    )
}

export function MessageRight(props: Props) {
    const {message, userName} = props

    return (
        <div className={classes.messageRightWrapper}>
            <div className={classes.messageRightContent}>
                <div className={classes.messageRightAuthor}>{userName + ":"}</div>
                <div className={classes.messageRightText}>
                    <p>{message}</p>
                </div>
            </div>
        </div>
    )
}

// export function MessageLeft(props: Props) {
//     const { message, userName } = props

//     return (
//         <Box sx={{ display: 'flex' }} className='messageRow'>
//             <Box sx={{ maxWidth: '100%' }}>
//                 <Box className='displayName'>{userName}</Box>
//                 <Box className='messageText'
//                     sx={{
//                         position: "relative",
//                         marginLeft: "20px",
//                         marginBottom: "10px",
//                         padding: "10px",
//                         backgroundColor: "#A8DDFD",
//                         width: "60%",
//                         //height: "50px",
//                         textAlign: "left",
//                         font: "400 .9em 'Open Sans', sans-serif",
//                         border: "1px solid #97C6E3",
//                         borderRadius: "10px",
//                         wordWrap: 'break-word',
//                         "&:after": {
//                             content: "''",
//                             position: "absolute",
//                             width: "0",
//                             height: "0",
//                             borderTop: "15px solid #A8DDFD",
//                             borderLeft: "15px solid transparent",
//                             borderRight: "15px solid transparent",
//                             top: "0",
//                             left: "-15px"
//                         },
//                         "&:before": {
//                             content: "''",
//                             position: "absolute",
//                             width: "0",
//                             height: "0",
//                             borderTop: "17px solid #97C6E3",
//                             borderLeft: "16px solid transparent",
//                             borderRight: "16px solid transparent",
//                             top: "-1px",
//                             left: "-17px"
//                         }
//                     }}
//                 >

//                     <Typography sx={{ padding: 0, margin: 0 }}>{message}</Typography>

//                 </Box>
//             </Box>

//         </Box>
//     )
// }

// export function MessageRight(props: Props) {
//     const { message, userName } = props

//     return (
//         <Box sx={{ display: 'flex', justifyContent: 'flex-end' }} className='messageRow'>
//             <Box sx={{ maxWidth: "100%" , display: 'flex', justifyContent: 'flex-end' }}>
//                 <Box className='messageText'
//                     sx={{
//                         position: "relative",
//                         marginRight: "20px",
//                         marginBottom: "10px",
//                         padding: "10px",
//                         backgroundColor: "#f8e896",
//                         width: "60%",
//                         //height: "50px",
//                         textAlign: "left",
//                         font: "400 .9em 'Open Sans', sans-serif",
//                         border: "1px solid #dfd087",
//                         borderRadius: "10px",
//                         wordWrap: 'break-word',
//                         "&:after": {
//                             content: "''",
//                             position: "absolute",
//                             width: "0",
//                             height: "0",
//                             borderTop: "15px solid #f8e896",
//                             borderLeft: "15px solid transparent",
//                             borderRight: "15px solid transparent",
//                             top: "0",
//                             right: "-15px"
//                         },
//                         "&:before": {
//                             content: "''",
//                             position: "absolute",
//                             width: "0",
//                             height: "0",
//                             borderTop: "17px solid #dfd087",
//                             borderLeft: "16px solid transparent",
//                             borderRight: "16px solid transparent",
//                             top: "-1px",
//                             right: "-17px"
//                         }
//                     }}
//                 >
//                     <Typography sx={{ padding: 0, margin: 0 }}>message</Typography>
//                 </Box>
//             </Box>
//         </Box>
//     )
// }


