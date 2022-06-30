
import LogoDesc from '../../img/Logo-Desc.svg';
import classes from './langing.module.scss';

export const LandingPage = () => {
    return (
     <div className={classes.box}>
        <div className={classes.logoDesc}>
            <LogoDesc />
        </div>
    </div>
    );
}